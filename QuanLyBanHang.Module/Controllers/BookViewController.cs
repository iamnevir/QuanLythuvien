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
        TargetViewType = ViewType.DetailView;
        TargetObjectType = typeof(LoanCard);
        SimpleAction choThueAction = new(this, "ChoThueAction", PredefinedCategory.View)
        {
            Caption = "Cho Thuê",
            ConfirmationMessage = "Bạn có muốn kích hoạt thẻ mượn này không?",
            ImageName = "BO_Task"
        };

        choThueAction.Execute += ChoThueAction_Execute;
    }
    private void ChoThueAction_Execute(object sender, SimpleActionExecuteEventArgs e)
    {
        if (((LoanCard)View.CurrentObject).Active == false)
        {
            ((LoanCard)View.CurrentObject).Active = true;
        }
    }
}
