using DevExpress.CodeParser;
using DevExpress.Data;
using DevExpress.DirectX.Common.DirectWrite;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.Model.Core;
using DevExpress.ExpressApp.Model.NodeGenerators;
using DevExpress.XtraScheduler.Native;
using DevExpress.XtraSpreadsheet.Model.History;
using QuanLyBanHang.Module.Extension;
using Microsoft.CodeAnalysis.Simplification;
using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace QuanLyBanHang.Module.Extension;

public class ModelNodeDetailController : ModelNodesGeneratorUpdater<ModelDetailViewLayoutNodesGenerator>
{
    public override void UpdateNode(ModelNode node)
    {
        // Cast the 'node' parameter to IModelLayout
        // to access the Layout node.        

    }
}

public class ModelNodeController : ModelNodesGeneratorUpdater<ModelViewsNodesGenerator>
{
    public override void UpdateNode(ModelNode viewsNode)
    {
        //TODO: dùng custom attribute để điều chỉnh việc sinh application model

        var classes = viewsNode.Application.BOModel.Where(bom =>
            bom.Name.Contains("BusinessObjects") &&
            Type.GetType(bom.Name).GetCustomAttributes(typeof(CustomDetailViewAttribute)).Any());
        foreach (var c in classes)
        {
            var type = Type.GetType(c.Name);
            CustomDetailView(c, type, viewsNode);
        }

        foreach (var modelClass in viewsNode.Application.BOModel
            .Where(bom => bom.Name.Contains("BusinessObjects")))
        {
            var type = Type.GetType(modelClass.Name);
            if (type != null)
            {
                CustomRootListView(modelClass, type, viewsNode);
                CustomNestedListView(modelClass, type, viewsNode);
                Readonly(modelClass, type);
            }
        }
    }

    /// <summary>
    /// Xử lý CustomDetailViewAttribute
    /// </summary>
    /// <param name="modelClass"></param>
    /// <param name="type"></param>
    /// <param name="viewsNode"></param>
    void CustomDetailView(IModelClass modelClass, Type type, ModelNode viewsNode)
    {
        var attrs = type.GetCustomAttributes(typeof(CustomDetailViewAttribute));
        foreach (CustomDetailViewAttribute attr in attrs.Cast<CustomDetailViewAttribute>())
        {
            //var attr = type.GetCustomAttribute<AdditionalDetailViewAttribute>();
            if (attr != null)
            {
                //var bom = viewsNode.Application.BOModel.GetClass(type);
                IModelDetailView detailViewNode;
                if (string.IsNullOrEmpty(attr.ViewId))
                {
                    detailViewNode = modelClass.DefaultDetailView;
                }
                else
                {
                    detailViewNode = viewsNode.AddNode<IModelDetailView>(attr.ViewId);
                    detailViewNode.ModelClass = modelClass;
                }

                foreach (var f in attr.FieldsReadonly)
                {
                    detailViewNode.Items[f].SetValue("AllowEdit", false);
                }

                foreach (var f in attr.FieldsToRemove)
                {
                    detailViewNode.Items[f].Remove();
                }

                detailViewNode.AllowDelete = attr.AllowDelete;
                detailViewNode.AllowEdit = attr.AllowEdit;
                detailViewNode.AllowNew = attr.AllowNew;

                if (attr.Tabbed)
                {
                    if (detailViewNode.GetNode("Layout") is IModelViewLayout layout)
                    {
                        var layoutNode = layout as ModelNode;
                        var mainGroup = layout.GetNode("Main") as IModelLayoutGroup;
                        mainGroup.ShowCaption = true;
                        mainGroup.Caption = "Chi tiết";
                        var mainNode = mainGroup as ModelNode;

                        if (mainGroup.GetNode("Tabs") is IModelTabbedGroup tabsGroup)
                        {
                            foreach (var tab in tabsGroup.GetNodes<IModelLayoutGroup>())
                            {
                                tab.Index++;
                            }
                            var tabsNode = tabsGroup as ModelNode;
                            layoutNode.AddClonedNode(tabsNode, tabsNode.Id);
                            tabsGroup.Remove();

                            tabsNode = layout.GetNode("Tabs") as ModelNode;
                            tabsNode.AddClonedNode(mainNode, mainNode.Id);

                        }
                        else
                        {
                            var tabsNode = layout.AddNode<IModelTabbedGroup>("Tabs") as ModelNode;
                            var lastNode = mainGroup.GetNode(mainGroup.NodeCount - 1) as ModelNode;
                            lastNode.Index = 1;
                            tabsNode.AddClonedNode(lastNode, lastNode.Id);
                            ((IModelLayoutGroup)lastNode).Remove();
                            tabsNode.AddClonedNode(mainNode, mainNode.Id);
                        }
                        mainGroup.Remove();
                    }
                }
            }
        }
    }

    /// <summary>
    /// Xử lý chung cho root và nested list view
    /// </summary>
    /// <param name="listviewNode"></param>
    /// <param name="attr"></param>
    /// <param name="viewsNode"></param>
    void CustomListView(IModelListView listviewNode, CustomListViewAttribute attr, ModelNode viewsNode)
    {
        //TODO: chỉ định detail view cho list view
        if (!string.IsNullOrEmpty(attr.DetailViewId))
        {
            var detailView = viewsNode.GetNode(attr.DetailViewId) as IModelDetailView;
            listviewNode.DetailView = detailView;
        }

        //TODO: chỉ định group summary
        listviewNode.GroupSummary = attr.GroupSummary;

        //TODO: chỉ định các tính năng thêm sửa xóa link
        listviewNode.AllowDelete = attr.AllowDelete;
        listviewNode.AllowEdit = attr.AllowEdit;
        listviewNode.AllowNew = attr.AllowNew;
        listviewNode.AllowLink = attr.AllowLink;
        listviewNode.AllowUnlink = attr.AllowUnlink;

        //TODO: ẩn các trường
        foreach (var f in attr.FieldsToHide)
            listviewNode.Columns[f].Index = -1;

        //TODO: sắp xếp
        for (var i = 0; i < attr.FieldsToSort.Length; i++)
        {
            var field = attr.FieldsToSort[i];
            if (field.EndsWith("."))
            {
                var f = field.TrimEnd('.');
                listviewNode.Columns[f].SortIndex = i;
                listviewNode.Columns[f].SortOrder = ColumnSortOrder.Descending;
            }
            else listviewNode.Columns[field].SortIndex = i;
        }

        //TODO: nhóm cột
        for (var i = 0; i < attr.FieldsToGroup.Length; i++)
            listviewNode.Columns[attr.FieldsToGroup[i]].GroupIndex = i;

        //TODO: xóa cột
        foreach (var f in attr.FieldsToRemove)
            listviewNode.Columns[f].Remove();

        //TODO: thêm dòng tổng
        foreach (var field in attr.FieldsToSum)
        {
            if (field.Contains(':'))
            {
                var items = field.Split(":", StringSplitOptions.TrimEntries & StringSplitOptions.RemoveEmptyEntries);
                var col = items[0];
                var sums = items[1].Split(",", StringSplitOptions.TrimEntries & StringSplitOptions.RemoveEmptyEntries);
                foreach (var s in sums)
                {
                    var column = listviewNode.Columns[col];
                    if (column != null)
                    {
                        var sum = column.Summary.AddNode<IModelColumnSummaryItem>(s);
                        sum.SummaryType = s.ToLower() switch
                        {
                            "count" => SummaryType.Count,
                            "sum" => SummaryType.Sum,
                            "average" => SummaryType.Average,
                            "min" => SummaryType.Min,
                            "max" => SummaryType.Max,
                            _ => SummaryType.Count
                        };
                    }
                }
            }
            else
            {
                var column = listviewNode.Columns[field];
                var sum = column.Summary.AddNode<IModelColumnSummaryItem>("Count");
                sum.SummaryType = SummaryType.Count;
            }
        }
    }

    /// <summary>
    /// Xử lý CustomListViewAttribute
    /// </summary>
    /// <param name="modelClass"></param>
    /// <param name="type"></param>
    /// <param name="viewsNode"></param>
    void CustomRootListView(IModelClass modelClass, Type type, ModelNode viewsNode)
    {
        var attrs = type.GetCustomAttributes(typeof(CustomRootListViewAttribute));
        foreach (CustomRootListViewAttribute attr in attrs.Cast<CustomRootListViewAttribute>())
        {
            //var attr = type.GetCustomAttribute<CustomListViewAttribute>();
            if (attr != null)
            {
                //var bom = viewsNode.Application.BOModel.GetClass(type);
                IModelListView listviewNode;
                if (string.IsNullOrEmpty(attr.ViewId))
                {
                    listviewNode = modelClass.DefaultListView;
                }
                else
                {
                    listviewNode = viewsNode.AddNode<IModelListView>(attr.ViewId);
                    listviewNode.ModelClass = modelClass;
                }

                if (listviewNode != null)
                {
                    CustomListView(listviewNode, attr, viewsNode);
                }
            }
        }
    }

    /// <summary>
    /// Xử lý CustomNestedListViewAttribute
    /// </summary>
    /// <param name="modelClass"></param>
    /// <param name="type"></param>
    /// <param name="viewsNode"></param>
    void CustomNestedListView(IModelClass modelClass, Type type, ModelNode viewsNode)
    {
        var attrs = type.GetCustomAttributes(typeof(CustomNestedListViewAttribute));
        foreach (CustomNestedListViewAttribute attr in attrs.Cast<CustomNestedListViewAttribute>())
        {
            if (attr != null)
            {
                IModelListView listviewNode;
                var listviewId = $"{type.Name}_{attr.CollectionProperty}_ListView";
                listviewNode = viewsNode.GetNode(listviewId) as IModelListView;

                if (listviewNode != null)
                {
                    CustomListView(listviewNode, attr, viewsNode);
                }
            }
        }
    }

    /// <summary>
    /// Xử lý ReadonlyAttribute
    /// </summary>
    /// <param name="modelClass"></param>
    /// <param name="type"></param>
    void Readonly(IModelClass modelClass, Type type)
    {
        // nếu attribute áp dụng với properties
        var props = type.GetProperties().Where(pi => pi.GetCustomAttribute<ReadonlyAttribute>() != null);
        foreach (var prop in props)
        {
            modelClass.FindOwnMember(prop.Name).AllowEdit = false;
        }
        // nếu attribute áp dụng với class
        var attr = type.GetCustomAttribute<ReadonlyAttribute>();
        if (attr != null)
        {
            if (attr.IsReversed)
            {
                foreach (var member in modelClass.OwnMembers)
                {
                    if (attr.Fields.Contains(member.Name))
                        member.AllowEdit = true;
                    else
                        member.AllowEdit = false;
                }
            }
            else
            {
                foreach (string field in attr.Fields)
                {
                    modelClass.FindOwnMember(field).AllowEdit = false;
                }
            }
        }
    }
}
