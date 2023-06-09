﻿using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp;
using System;

namespace QuanLyBanHang.Module.Controllers;

public partial class ConfirmationWindowActionController : ViewController
{
    DeleteObjectsViewController deleteObjectsViewController;
    protected override void OnActivated()
    {
        base.OnActivated();
        deleteObjectsViewController = Frame.GetController<DeleteObjectsViewController>();
        if (deleteObjectsViewController != null)
        {
            View.SelectionChanged += View_SelectionChanged;
            SetConfirmationMessage();
        }
    }
    void View_SelectionChanged(object sender, EventArgs e)
    {
        SetConfirmationMessage();
    }

    private void SetConfirmationMessage()
    {
        deleteObjectsViewController.DeleteAction.ConfirmationMessage = String.Format("Bạn sắp xóa {0} đối tượng. Bạn có muốn tiếp tục?", View.SelectedObjects.Count);
    }

    protected override void OnDeactivated()
    {
        base.OnDeactivated();
        if (deleteObjectsViewController != null)
        {
            View.SelectionChanged -= View_SelectionChanged;
            deleteObjectsViewController = null;
        }
    }
}