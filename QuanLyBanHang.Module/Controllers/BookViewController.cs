using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using QuanLyBanHang.Module.BusinessObjects;

namespace QuanLyBanHang.Module.Controllers;


public partial class BookViewController : ViewController
{

    public BookViewController()
    {
        InitializeComponent();
        TargetViewType = ViewType.DetailView;
        TargetObjectType = typeof(LoanCard);
        SimpleAction clearBookAction = new(this, "ClearBookAction", DevExpress.Persistent.Base.PredefinedCategory.View)
        {
            Caption = "Clear",
            ConfirmationMessage = "Bạn có muốn xóa sạch sách không?",
            ImageName = "Action_Clear"
        };
        clearBookAction.Execute += ClearBookAction_Execute;

    }

    private void ClearBookAction_Execute(object sender, SimpleActionExecuteEventArgs e)
    {
        while (((LoanCard)View.CurrentObject).Books.Count > 0)
        {
            ((LoanCard)View.CurrentObject).Books.Remove(((LoanCard)View.CurrentObject).Books[0]);
            ObjectSpace.SetModified(View.CurrentObject,View.ObjectTypeInfo.FindMember(nameof(LoanCard.Books)));
        }
    }
}
