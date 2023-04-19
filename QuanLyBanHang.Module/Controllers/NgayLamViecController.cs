using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.Persistent.Base;
using QuanLyBanHang.Module.BusinessObjects;

namespace QuanLyBanHang.Module.Controllers;


public partial class NgayLamViecController : ViewController
{
    
    public NgayLamViecController()
    {
        InitializeComponent();
        Btn_LapDanhSach();
    }
    public void Btn_LapDanhSach()
    {
        var lapDanhSachAction = new PopupWindowShowAction(this, $"{Btn_LapDanhSach}", PredefinedCategory.Edit)
        {
            Caption = "Lập danh sách",
            ConfirmationMessage = "Bạn có muốn lập danh sách không?",
            ImageName = "Action_Inline_New",
            TargetViewNesting = Nesting.Root,
            TargetViewType = ViewType.ListView,
            TargetObjectType = typeof(NgayLamViec),
            SelectionDependencyType = SelectionDependencyType.RequireMultipleObjects
        };
        lapDanhSachAction.Execute += LapDanhSachAction_Execute;
        lapDanhSachAction.CustomizePopupWindowParams += LapDanhSachAction_CustomizePopupWindowParams;
    }

    private void LapDanhSachAction_CustomizePopupWindowParams(object sender, CustomizePopupWindowParamsEventArgs e)
    {
        IObjectSpace objectSpace = e.Application.CreateObjectSpace(typeof(NgayLamViec));
        var currentObject = objectSpace.GetObject((NgayLamViec)View.CurrentObject);
        DetailView detailView = e.Application.CreateDetailView(objectSpace, currentObject);
        detailView.ViewEditMode = ViewEditMode.Edit;
        e.View = detailView;
    }

    private void LapDanhSachAction_Execute(object sender, PopupWindowShowActionExecuteEventArgs e)
    {
        throw new NotImplementedException();
    }
}
