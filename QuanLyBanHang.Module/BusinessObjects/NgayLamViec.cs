﻿using DevExpress.ExpressApp;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using QuanLyBanHang.Module.Extension;
using System.Drawing;

namespace QuanLyBanHang.Module.BusinessObjects;

[DefaultClassOptions]
[ImageName("Calendar")]
[NavigationItem(Generate.Employee)]
[LookupEditorMode(LookupEditorMode.AllItemsWithSearch)]
[ListViewFindPanel(true)]
[XafDisplayName("Ngày Làm việc")]
[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
[Appearance("NgayNghi", AppearanceItemType = "ViewItem", TargetItems = "*", Context = "Any",
        Criteria = "NgayNghi=true", FontColor = "Red", FontStyle = FontStyle.Bold, Priority = 1)]

[CustomNestedListView(nameof(ChamCongs), AllowUnlink = false, AllowLink = false)]

[ListViewFilter("Tất Cả", "", Index = 0)]
[ListViewFilter("Hôm Nay", "GetDate([Ngay])=LocalDateTimeToday()", Index = 1)]
[ListViewFilter("Tháng Này", "[Thang]=GetMonth(LocalDateTimeToday())", Index = 2)]
[ListViewFilter("Tuần Này", "IsThisWeek(Ngay)=true", Index = 3)]
[ListViewFilter("Ngày Nghỉ", "NgayNghi=true", Index = 4)]
public class NgayLamViec : BaseObject
{

    public NgayLamViec(Session session)
        : base(session)
    {
    }

    public override void AfterConstruction()
    {
        base.AfterConstruction();
        
    }

    string note;
    bool ngayNghi;
    int nam;
    int thang;
    int tuan;
    string thu;
    DateTime ngay;
    [XafDisplayName("Ngày")]
    public DateTime Ngay
    {
        get => ngay;
        set => SetPropertyValue(nameof(Ngay), ref ngay, value);
    }
    [XafDisplayName("Thứ")]
    public string Thu
    {
        get => thu;
        set => SetPropertyValue(nameof(Thu), ref thu, value);
    }
    [XafDisplayName("Tuần")]
    public int Tuan
    {
        get => tuan;
        set => SetPropertyValue(nameof(Tuan), ref tuan, value);
    }
    [XafDisplayName("Tháng")]
    public int Thang
    {
        get => thang;
        set => SetPropertyValue(nameof(Thang), ref thang, value);
    }
    [XafDisplayName("Năm")]
    public int Nam
    {
        get => nam;
        set => SetPropertyValue(nameof(Nam), ref nam, value);
    }
    [ModelDefault("AllowEdit", "false")]
    [XafDisplayName("Ngày nghỉ")]
    public bool NgayNghi
    {
        get => ngayNghi;
        set => SetPropertyValue(nameof(NgayNghi), ref ngayNghi, value);
    }
    [XafDisplayName("Ghi chú")]
    public string Note
    {
        get => note;
        set => SetPropertyValue(nameof(Note), ref note, value);
    }
    [Action(ToolTip = "Kích hoạt ngày nghỉ", Caption = "Ngày nghỉ")]
    public void StatusChanged()
    {
        ngayNghi = true;
    }
    [XafDisplayName("Chấm Công")]
    [Association("NgayLamViec-ChamCongs")]
    public XPCollection<ChamCong> ChamCongs
    {
        get
        {
            return GetCollection<ChamCong>(nameof(ChamCongs));
        }
    }
}