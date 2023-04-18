using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Blazor.Editors.Adapters;
using DevExpress.ExpressApp.Blazor.Editors;
using QuanLyBanHang.Module.BusinessObjects;

namespace QuanLyBanHang.Blazor.Server.Controllers
{
    
    public partial class DateEditController : ObjectViewController<DetailView, object>
    {
        public DateEditController()
        {
            InitializeComponent();
        }
        protected override void OnActivated()
        {
            base.OnActivated();
            View.CustomizeViewItemControl<DateTimePropertyEditor>(this, SetCalendarView, nameof(Employee.BirthDay));
            View.CustomizeViewItemControl<DateTimePropertyEditor>(this, SetCalendarView, nameof(LoanCard.ActualPaymentDate));
            View.CustomizeViewItemControl<DateTimePropertyEditor>(this, SetCalendarView, nameof(LoanCard.ExpectedPaymentDate));
            View.CustomizeViewItemControl<DateTimePropertyEditor>(this, SetCalendarView, nameof(Book.CreatedDate));
            View.CustomizeViewItemControl<DateTimePropertyEditor>(this, SetCalendarView, nameof(Student.BirthDay));

        }
        private void SetCalendarView(DateTimePropertyEditor propertyEditor)
        {
            var dateEditAdapter = (DxDateEditAdapter)propertyEditor.Control;
            dateEditAdapter.ComponentModel.PickerDisplayMode = DevExpress.Blazor.DatePickerDisplayMode.ScrollPicker;
        }
    }
}
