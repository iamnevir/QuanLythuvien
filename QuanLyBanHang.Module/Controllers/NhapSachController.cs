using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using QuanLyBanHang.Module.BusinessObjects;

namespace QuanLyBanHang.Module.Controllers;


public partial class NhapSachController : ViewController
{

    public NhapSachController()
    {
        InitializeComponent();
        Btn_NhapSach();
    }
    public void Btn_NhapSach()
    {
        var action = new PopupWindowShowAction(this, "nhapsach", PredefinedCategory.Edit)
        {
            Caption = "Nhập thêm sách",
            ImageName = "BO_Document",
            TargetViewNesting = Nesting.Root,
            TargetViewType = ViewType.ListView,
            TargetObjectType = typeof(Book),
            SelectionDependencyType = SelectionDependencyType.RequireSingleObject
        };
        action.CustomizePopupWindowParams += Action_CustomizePopupWindowParams;
        action.Execute += Action_Execute;
    }

    private void Action_Execute(object sender, PopupWindowShowActionExecuteEventArgs e)
    {
        int soluong = ((NhapSach)e.PopupWindowViewCurrentObject).Soluong;
        foreach (Book item in View.SelectedObjects)
        {
            item.Quantity = soluong;
        }
        ObjectSpace.CommitChanges();
        Application.ShowViewStrategy.ShowMessage("Nhập thêm sách thành công!", InformationType.Success);
    }

    private void Action_CustomizePopupWindowParams(object sender, CustomizePopupWindowParamsEventArgs e)
    {
        NonPersistentObjectSpace os = (NonPersistentObjectSpace)e.Application.CreateObjectSpace(typeof(NhapSach));
        os.PopulateAdditionalObjectSpaces(Application);
        e.DialogController.SaveOnAccept = false;
        e.View = e.Application.CreateDetailView(os, os.CreateObject<NhapSach>());
    }

}
[DomainComponent]
public class NhapSach
{
    [XafDisplayName("Số lượng")]
    public int Soluong { get; set; }
}