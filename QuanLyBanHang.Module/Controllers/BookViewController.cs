using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.Persistent.Base;
using QuanLyBanHang.Module.BusinessObjects;

namespace QuanLyBanHang.Module.Controllers;
public partial class BookViewController : ViewController
{

    public BookViewController()
    {
        InitializeComponent();
        Btn_ChoThue();
    }
    public void Btn_ChoThue()
    {
        var choThueAction = new SimpleAction(this, "ChoThueAction", PredefinedCategory.Edit)
        {
            Caption = "Cho Thuê",
            ConfirmationMessage = "Bạn có muốn kích hoạt cho thuê thẻ mượn này không?",
            ImageName = "BO_Task",
            TargetViewNesting = Nesting.Root,
            TargetViewType = ViewType.Any,
            TargetObjectType = typeof(LoanCard),
            SelectionDependencyType = SelectionDependencyType.RequireMultipleObjects
        };
        choThueAction.Execute += (s, e) =>
        {
            foreach (LoanCard item in View.SelectedObjects)
            {
                if (item.Active == true)
                {
                    choThueAction.Enabled["DisableAction"] = false;
                }
                else
                {
                    item.Active = true;
                }
                
            }
            ObjectSpace.CommitChanges();
            Application.ShowViewStrategy.ShowMessage("Xác nhận cho thuê thành công!", InformationType.Success);
        };
    }
}
