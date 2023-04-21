using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.Persistent.Base;
using QuanLyBanHang.Module.BusinessObjects;

namespace QuanLyBanHang.Module.Controllers;


public partial class BangChamCongController : ObjectViewController<ListView,ChamCong>
{
    
    public BangChamCongController()
    {
        InitializeComponent();
        Btn_LapBangChamCong();
    }
    public void Btn_LapBangChamCong()
    {
        var action = new SimpleAction(this, "LapBangChamCong", PredefinedCategory.Edit)
        {
            Caption = "Lập bảng chấm công",
            ImageName = "Action_Inline_New",
            TargetViewNesting = Nesting.Nested,
            TargetViewType = ViewType.ListView,
            TargetObjectType = typeof(ChamCong),
            SelectionDependencyType = SelectionDependencyType.Independent,
        };
        action.Execute += Action_Execute;

    }

    private void Action_Execute(object sender, SimpleActionExecuteEventArgs e)
    {
        if (((DetailView)ObjectSpace.Owner).CurrentObject is NgayLamViec nlv)
        {
            var nhanvien = ObjectSpace.GetObjectsQuery<Employee>().ToList();
            if (nhanvien?.Any() != true)
            {
                Application.ShowViewStrategy.ShowMessage("Chưa có nhân viên nào!", InformationType.Error);
            }
            else
            {
                var ngaylamviec = ObjectSpace.GetObject(nlv);
                ChamCong chamCong = ObjectSpace.FirstOrDefault<ChamCong>(p => p.Thu == ngaylamviec.Thu);
                if (chamCong != null)
                {
                    Application.ShowViewStrategy.ShowMessage("Đã có bảng chấm công rồi!", InformationType.Error);
                }
                else
                {
                    foreach (var item in nhanvien)
                    {
                        ChamCong cc = ObjectSpace.CreateObject<ChamCong>();
                        cc.NgayLamViec = ngaylamviec;
                        cc.Thu = ngaylamviec.Thu;
                        cc.Tuan = ngaylamviec.Tuan;
                        cc.Thang = ngaylamviec.Thang;
                        cc.Nam = ngaylamviec.Nam;
                        cc.Employee = item;
                        cc.Note = string.Empty;
                        cc.Sang = DiemChamCong.chuadiemdanh;
                        cc.Chieu = DiemChamCong.chuadiemdanh;
                        View.CollectionSource.Add(cc);
                    }
                    ObjectSpace.CommitChanges();
                    View.Refresh();
                    Application.ShowViewStrategy.ShowMessage("Lập bảng chấm công thành công!", InformationType.Success);
                }
            }
        }
    }
}
