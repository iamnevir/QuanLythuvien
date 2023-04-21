using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using QuanLyBanHang.Module.BusinessObjects;
using System.Globalization;

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
            ImageName = "Action_Inline_New",
            TargetViewNesting = Nesting.Root,
            TargetViewType = ViewType.ListView,
            TargetObjectType = typeof(NgayLamViec),
            SelectionDependencyType = SelectionDependencyType.Independent
        };
        lapDanhSachAction.Execute += LapDanhSachAction_Execute;
        lapDanhSachAction.CustomizePopupWindowParams += LapDanhSachAction_CustomizePopupWindowParams;
    }

    private void LapDanhSachAction_CustomizePopupWindowParams(object sender, CustomizePopupWindowParamsEventArgs e)
    {
        NonPersistentObjectSpace os = (NonPersistentObjectSpace)e.Application.CreateObjectSpace(typeof(GenerateNgayLamViec));
        os.PopulateAdditionalObjectSpaces(Application);
        e.DialogController.SaveOnAccept = false;
        e.View = e.Application.CreateDetailView(os, os.CreateObject<GenerateNgayLamViec>());
    }

    private void LapDanhSachAction_Execute(object sender, PopupWindowShowActionExecuteEventArgs e)
    {
        int year = ((GenerateNgayLamViec)e.PopupWindowViewCurrentObject).Nam;
        List<DateTime> dates = new();
        for (int i = 1; i < 13; i++)
        {
            var date = GetDates(year, i);
            dates.AddRange(date);
        }
        List<string> thu = new() { "Thứ hai", "Thứ ba", "Thứ tư", "Thứ năm", "Thứ sáu", "Thứ bảy", "Chủ nhật" };
        foreach (var item in dates)
        {
            NgayLamViec nlv = ObjectSpace.CreateObject<NgayLamViec>();
            nlv.Ngay = item;
            CultureInfo myCI = new("en-US");
            Calendar myCal = myCI.Calendar;
            CalendarWeekRule myCWR = myCI.DateTimeFormat.CalendarWeekRule;
            DayOfWeek myFirstDOW = myCI.DateTimeFormat.FirstDayOfWeek;
            nlv.Tuan = myCal.GetWeekOfYear(item, myCWR, myFirstDOW);
            nlv.Nam = item.Year;
            nlv.Thu = item.ToString("dddd");
            nlv.Thang = item.Month;
            if (item.DayOfWeek == DayOfWeek.Sunday)
            {
                nlv.NgayNghi = true;
            }
        }
        ObjectSpace.CommitChanges();
        View.Refresh();
        Application.ShowViewStrategy.ShowMessage("Lập danh sách thành công!", InformationType.Success);
    }
    public static List<DateTime> GetDates(int year, int month)
    {
        return Enumerable.Range(1, DateTime.DaysInMonth(year, month))
                         .Select(day => new DateTime(year, month, day))
                         .ToList();
    }
}
[DomainComponent]
public class GenerateNgayLamViec
{
    [XafDisplayName("Năm")]
    public int Nam { get; set; }
}
