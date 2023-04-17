using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.Persistent.Base;
using QuanLyBanHang.Module.BusinessObjects;

namespace QuanLyBanHang.Module.Controllers;


public partial class ShowPopupViewController : ViewController
{

    public ShowPopupViewController()
    {
        InitializeComponent();
        var popupAction = new PopupWindowShowAction(this, "ShowPopup", PredefinedCategory.View)
        {
            SelectionDependencyType = SelectionDependencyType.RequireSingleObject
        };
        popupAction.CustomizePopupWindowParams += PopupAction_CustomizePopupWindowParams;
    }


    private void PopupAction_CustomizePopupWindowParams(object sender, CustomizePopupWindowParamsEventArgs e)
    {
        IObjectSpace objectSpace = e.Application.CreateObjectSpace(typeof(object));
        object currentObject = objectSpace.GetObject(View.CurrentObject);
        DetailView detailView = e.Application.CreateDetailView(objectSpace, currentObject);
        detailView.ViewEditMode = ViewEditMode.Edit;
        e.View = detailView;
    }
}