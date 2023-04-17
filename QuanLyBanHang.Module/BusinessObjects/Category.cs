using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using System.ComponentModel;

namespace QuanLyBanHang.Module.BusinessObjects;

[DefaultClassOptions]
[DefaultProperty(nameof(CategoryName))]
[DefaultListViewOptions(MasterDetailMode.ListViewOnly, true, NewItemRowPosition.Top)]
[ListViewFindPanel(true)]
[ListViewAutoFilterRow(true)]
[LookupEditorMode(LookupEditorMode.AllItemsWithSearch)]
[NavigationItem(Generate.Book)]
[XafDisplayName("Danh mục sách")]
[ImageName("BO_Category")]
public class Category : BaseObject
{ 
    public Category(Session session)
        : base(session)
    {
    }
    public override void AfterConstruction()
    {
        base.AfterConstruction();
    }
   
    string description;
    string categoryName;
    [XafDisplayName("Tên Danh mục")]
    [RuleRequiredField]
    [RuleUniqueValue]
    public string CategoryName
    {
        get => categoryName;
        set => SetPropertyValue(nameof(CategoryName), ref categoryName, value);
    }

    [XafDisplayName("Mô Tả Danh mục")]
    public string Description
    {
        get => description;
        set => SetPropertyValue(nameof(Description), ref description, value);
    }

    [Association("Category-Books")]
    public XPCollection<Book> Books
    {
        get => GetCollection<Book>(nameof(Books));
    }
}