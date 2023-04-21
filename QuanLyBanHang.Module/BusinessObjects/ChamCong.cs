using DevExpress.ExpressApp;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using DevExpress.XtraPrinting.Native;
using System.Drawing;

namespace QuanLyBanHang.Module.BusinessObjects;

[DefaultClassOptions]
[ImageName("ClearTableStyle")]
[NavigationItem(Generate.Employee)]
[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.Top)]
[LookupEditorMode(LookupEditorMode.AllItemsWithSearch)]
[ListViewFindPanel(true)]
[XafDisplayName("Chấm Công")]
[Appearance("ChamSang", AppearanceItemType = "ViewItem", TargetItems = "Sang", Context = "Any",
    Criteria = "Sang = 4",
FontColor = "Red", FontStyle = FontStyle.Bold, Priority = 1)]
[Appearance("ChamChieu", AppearanceItemType = "ViewItem", TargetItems = "Chieu", Context = "Any",
    Criteria = "Chieu = 4",
FontColor = "Red", FontStyle = FontStyle.Bold, Priority = 1)]
public class ChamCong : BaseObject
{ 
    public ChamCong(Session session)
        : base(session)
    {
    }
    public override void AfterConstruction()
    {
        base.AfterConstruction();
        
    }

    NgayLamViec ngayLamViec;
    bool nghiPhep;
    Employee employee;
    int nam;
    int thang;
    int tuan;
    string thu;
    DiemChamCong chieu;
    DiemChamCong sang;
    int ngoaiGio;
    string note;
    double cong;
    [Association("NgayLamViec-ChamCongs")]
    [VisibleInListView(false)]
    [VisibleInDetailView(false)]
    public NgayLamViec NgayLamViec
    {
        get => ngayLamViec;
        set => SetPropertyValue(nameof(NgayLamViec), ref ngayLamViec, value);
    }
    [XafDisplayName("Tháng")]
    public int Thang
    {
        get => thang;
        set => SetPropertyValue(nameof(Thang), ref thang, value);
    }
    [XafDisplayName("Tuần")]
    public int Tuan
    {
        get => tuan;
        set => SetPropertyValue(nameof(Tuan), ref tuan, value);
    }
    [XafDisplayName("Thứ")]
    public string Thu
    {
        get => thu;
        set => SetPropertyValue(nameof(Thu), ref thu, value);
    }
    [XafDisplayName("Năm")]
    public int Nam
    {
        get => nam;
        set => SetPropertyValue(nameof(Nam), ref nam, value);
    }
    [XafDisplayName("Nhân viên")]
    public Employee Employee
    {
        get => employee;
        set => SetPropertyValue(nameof(Employee), ref employee, value);
    }

    [XafDisplayName("Công")]
    public double Cong
    {
        get
        {
            if (!IsLoading && !IsSaving)
            {
                double tinhcong = 0;
                switch (Sang)
                {
                    case DiemChamCong.vang:
                        tinhcong += 0;
                        break;
                    case DiemChamCong.nghiphep:
                        tinhcong += 0;
                        break;
                    case DiemChamCong.densomvemuon:
                        tinhcong += 0.25;
                        break;
                    case DiemChamCong.chuadiemdanh:
                        tinhcong += 0;
                        break;
                    case DiemChamCong.comat:
                        tinhcong += 0.5;
                        break;
                }
                switch (Chieu)
                {
                    case DiemChamCong.vang:
                        tinhcong += 0;
                        break;
                    case DiemChamCong.nghiphep:
                        tinhcong += 0;
                        break;
                    case DiemChamCong.densomvemuon:
                        tinhcong += 0.25;
                        break;
                    case DiemChamCong.chuadiemdanh:
                        tinhcong += 0;
                        break;
                    case DiemChamCong.comat:
                        tinhcong += 0.5;
                        break;
                }
                return tinhcong;
            }
            return cong;
        }
        set => SetPropertyValue(nameof(Cong), ref cong, value);
    }
    [XafDisplayName("Ghi Chú")]
    public string Note
    {
        get => note;
        set => SetPropertyValue(nameof(Note), ref note, value);
    }
    [XafDisplayName("Ngoài giờ")]
    public int NgoaiGio
    {
        get => ngoaiGio;
        set => SetPropertyValue(nameof(NgoaiGio), ref ngoaiGio, value);
    }
    [XafDisplayName("Sáng")]
    public DiemChamCong Sang
    {
        get => sang;
        set => SetPropertyValue(nameof(Sang), ref sang, value);
    }
    [XafDisplayName("Chiều")]
    public DiemChamCong Chieu
    {
        get => chieu;
        set => SetPropertyValue(nameof(Chieu), ref chieu, value);
    }
    [ModelDefault("AllowEdit", "false")]
    [XafDisplayName("Nghỉ phép")]
    public bool NghiPhep
    {
        get => nghiPhep;
        set => SetPropertyValue(nameof(NghiPhep), ref nghiPhep, value);
    }
    [Action(ToolTip = "Kích hoạt nghỉ phép", Caption = "Nghỉ phép")]
    public void StatusChanged()
    {
        nghiPhep = true;
    }
}
public enum DiemChamCong
{
    [XafDisplayName("Vắng")]
    vang,
    [XafDisplayName("Có mặt")]
    comat,
    [XafDisplayName("Nghỉ Phép")]
    nghiphep,
    [XafDisplayName("Đi sớm về muộn")]
    densomvemuon,
    [XafDisplayName("Chưa điểm danh")]
    chuadiemdanh
}


