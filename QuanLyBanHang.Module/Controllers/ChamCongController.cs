//using DevExpress.ExpressApp;
//using DevExpress.ExpressApp.Actions;
//using DevExpress.Persistent.Base;
//using QuanLyBanHang.Module.BusinessObjects;
//namespace QuanLyBanHang.Module.Controllers;


//public partial class ChamCongController : ViewController
//{
    
//    public ChamCongController()
//    {
//        InitializeComponent();
//        Btn_ChamSang();
//        //Btn_ChamChieu();
//    }
//    public void Btn_ChamSang()
//    {
//        var action = new PopupWindowShowAction(this, $"{Btn_ChamSang}", PredefinedCategory.Edit)
//        {
//            Caption = "Chấm sáng",
//            ImageName = "Action_Validation_Validate",
//            TargetViewNesting = Nesting.Nested,
//            TargetViewType = ViewType.ListView,
//            TargetObjectType = typeof(ChamCong),
//            TargetObjectsCriteria= "[Sang] = ##Enum#DXApplication.Blazor.Common.Enum+DiemChamCong,chuadiemdanh#",
//            SelectionDependencyType = SelectionDependencyType.RequireMultipleObjects
//        };
//        action.CustomizePopupWindowParams += Action_CustomizePopupWindowParams;
//        action.Execute += Action_Execute;
//    }

//    private void Action_Execute(object sender, PopupWindowShowActionExecuteEventArgs e)
//    {
//        var chamCong = ((DiemChamCong)e.PopupWindowViewCurrentObject);
//        foreach(ChamCong item in View.SelectedObjects)
//        {
//            item.Sang = chamCong;
//        }
//        ObjectSpace.CommitChanges();
//        Application.ShowViewStrategy.ShowMessage("Chấm công thành công", InformationType.Success);
//    }
//    //public void Btn_ChamChieu()
//    //{
//    //    var action1 = new PopupWindowShowAction(this, $"{Btn_ChamChieu}", PredefinedCategory.Edit)
//    //    {
//    //        Caption = "Chấm chiều",
//    //        ImageName = "Action_Validation_Validate",
//    //        TargetViewNesting = Nesting.Nested,
//    //        TargetViewType = ViewType.ListView,
//    //        TargetObjectType = typeof(ChamCong),
//    //        TargetObjectsCriteria = "[Chieu] = ##Enum#DXApplication.Blazor.Common.Enum+DiemChamCong,chuadiemdanh#",
//    //        SelectionDependencyType = SelectionDependencyType.RequireMultipleObjects
//    //    };
//    //    action1.CustomizePopupWindowParams += Action_CustomizePopupWindowParams;
//    //    action1.Execute += Action1_Execute; 
//    //}

//    //private void Action1_Execute(object sender, PopupWindowShowActionExecuteEventArgs e)
//    //{
//    //    var chamCong = ((DiemChamCong)e.PopupWindowViewCurrentObject);
//    //    foreach (ChamCong item in View.SelectedObjects)
//    //    {
//    //        item.Sang = chamCong;
//    //    }
//    //    ObjectSpace.CommitChanges();
//    //    Application.ShowViewStrategy.ShowMessage("Chấm công thành công", InformationType.Success);
//    //}
//    private void Action_CustomizePopupWindowParams(object sender, CustomizePopupWindowParamsEventArgs e)
//    {
//        NonPersistentObjectSpace objectSpace = (NonPersistentObjectSpace)e.Application.CreateObjectSpace(typeof(DiemChamCong));
//        objectSpace.PopulateAdditionalObjectSpaces(Application);
//        e.DialogController.SaveOnAccept = false;
//        e.View =e.Application.CreateDetailView(objectSpace,objectSpace.CreateObject<DiemChamCong>());

//    }
//}
