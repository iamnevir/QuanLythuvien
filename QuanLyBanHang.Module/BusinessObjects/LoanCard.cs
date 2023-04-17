using DevExpress.ExpressApp;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using System.Drawing;

namespace QuanLyBanHang.Module.BusinessObjects;
[DefaultClassOptions]
[DefaultListViewOptions(MasterDetailMode.ListViewOnly, true, NewItemRowPosition.Top)]
[ListViewFindPanel(true)]
[ListViewAutoFilterRow(true)]
[LookupEditorMode(LookupEditorMode.AllItemsWithSearch)]
[NavigationItem(Generate.Loan)]
[RuleCriteria("ExpectedPaymentDate >= LoanDate",
    CustomMessageTemplate = "Ngày trả dự kiến phải lớn hơn ngày mượn")]
[RuleCriteria("ActualPaymentDate >= LoanDate",
    CustomMessageTemplate = "Ngày trả thực tế phải lớn hơn ngày mượn")]
[XafDisplayName("Thẻ Mượn")]
[Appearance("LoanCardObject", AppearanceItemType = "ViewItem", TargetItems = "*",
    Criteria = "Status=2", Context = "ListView", BackColor = "Green",
        FontColor = "White", FontStyle = FontStyle.Strikeout, Priority = 2)]
[Appearance("LoanCardObject1", AppearanceItemType = "ViewItem", TargetItems = "*",
    Criteria = "Status=1", Context = "ListView", BackColor = "Red",
        FontColor = "White", Priority = 2)]
[ImageName("Card")]
public class LoanCard : BaseObject
{
    public LoanCard(Session session)
        : base(session)
    {
    }
    public override void AfterConstruction()
    {
        base.AfterConstruction();
        Status = Status.ChưaTrả;
    }
    [Persistent("ExpectedAmount")]
    decimal expectedAmount;
    [Persistent("CreatedDate")]
    readonly DateTime createdDate = DateTime.Now;
    Employee employee;
    [Persistent("TotalLoanPrice")]
    decimal totalLoanPrice;
    decimal loanPrice;
    Student student;
    Status status;
    DateTime actualPaymentDate;
    DateTime expectedPaymentDate;
    DateTime loanDate = DateTime.Now;
    [PersistentAlias(nameof(totalLoanPrice))]
    [ModelDefault("DisplayFormat", "{0,-10:N0}Đ")]
    [XafDisplayName("Tổng tiền phải trả đến hiện tại")]
    public decimal TotalLoanPrice
    {
        get
        {
            if (!IsLoading && !IsSaving && totalLoanPrice == 0)
                UpdateTotalLoanPrice(false);
            return totalLoanPrice;
        }
    }
    [ModelDefault("DisplayFormat", "{0,-10:N0}Đ")]
    [XafDisplayName("Giá Mượn")]
    public decimal LoanPrice
    {
        get
        {
            if (!IsLoading && !IsSaving && loanPrice == 0)
                UpdateLoanPrice(false);
            return loanPrice;
        }
        set => SetPropertyValue(nameof(LoanPrice), ref loanPrice, value);
    }
    [ModelDefault("EditMask", "MMM/d/yyyy hh:mm tt")]
    [ModelDefault("DisplayFormat", "D")]
    [XafDisplayName("Ngày Mượn")]
    public DateTime LoanDate
    {
        get => loanDate;
        set => SetPropertyValue(nameof(LoanDate), ref loanDate, value);
    }
    [ModelDefault("EditMask", "MMM/d/yyyy hh:mm tt")]
    [ModelDefault("DisplayFormat", "D")]
    [XafDisplayName("Ngày trả dự kiến")]
    public DateTime ExpectedPaymentDate
    {
        get => expectedPaymentDate;
        set => SetPropertyValue(nameof(ExpectedPaymentDate), ref expectedPaymentDate, value);
    }
    [ModelDefault("DisplayFormat", "{0,-10:N0}Đ")]
    [XafDisplayName("Số tiền dự kiến phải trả")]
    public decimal ExpectedAmount
    {
        get
        {
            if (!IsLoading && !IsSaving && expectedAmount == 0)
                UpdateExpectedAmount(false);
            return expectedAmount;
        }
    }
    [ModelDefault("EditMask", "MMM/d/yyyy hh:mm tt")]
    [ModelDefault("DisplayFormat", "D")]
    [XafDisplayName("Ngày trả thực tế")]
    public DateTime ActualPaymentDate
    {
        get => actualPaymentDate;
        set => SetPropertyValue(nameof(ActualPaymentDate), ref actualPaymentDate, value);
    }
    [PersistentAlias(nameof(createdDate))]
    [XafDisplayName("Ngày tạo thẻ mượn")]
    public DateTime CreatedDate
    {
        get => createdDate;
    }

    [XafDisplayName("Trạng Thái")]
    public Status Status
    {
        get => status;
        set => SetPropertyValue(nameof(Status), ref status, value);
    }

    [Association("Student-LoanCards")]
    public Student Student
    {
        get => student;
        set => SetPropertyValue(nameof(Student), ref student, value);
    }

    [Association("LoanCard-Books")]
    public XPCollection<Book> Books
    {
        get => GetCollection<Book>(nameof(Books));
    }

    [Association("Employee-LoanCards")]
    public Employee Employee
    {
        get => employee;
        set => SetPropertyValue(nameof(Employee), ref employee, value);
    }
    public void UpdateLoanPrice(bool forceChangeEvents)
    {
        decimal? oldLoanPrice = loanPrice;
        decimal tempLoan = 0m;
        foreach (Book book in Books)
            tempLoan += book.LoanPrice;
        loanPrice = tempLoan;
        if (forceChangeEvents)
            OnChanged(nameof(LoanPrice), oldLoanPrice, loanPrice);
    }
    public void UpdateTotalLoanPrice(bool forceChangeEvents)
    {
        decimal? oldTotalLoanPrice = totalLoanPrice;
        totalLoanPrice = loanPrice * (DateTime.Now.Day - LoanDate.Day);
        if (forceChangeEvents)
            OnChanged(nameof(TotalLoanPrice), oldTotalLoanPrice, totalLoanPrice);
    }
    public void UpdateExpectedAmount(bool forceChangeEvents)
    {
        decimal? oldExpectedAmount = expectedAmount;
        expectedAmount = loanPrice * (ExpectedPaymentDate.Day - LoanDate.Day);
        if (forceChangeEvents)
            OnChanged(nameof(ExpectedAmount), oldExpectedAmount, expectedAmount);
    }
    [Action(ToolTip = "Gia hạn thẻ mượn thêm một ngày", Caption = "Gia hạn")]
    public void Postpone()
    {
        if (ExpectedPaymentDate == DateTime.MinValue)
        {
            ExpectedPaymentDate = DateTime.Now;
        }
        ExpectedPaymentDate += TimeSpan.FromDays(1);
    }
    protected override void OnLoaded()
    {
        Reset();
        base.OnLoaded();
    }
    private void Reset()
    {
        expectedAmount = 0;
        totalLoanPrice = 0;
        loanPrice = 0;
    }

}
public enum Status
{
    ChưaTrả,
    QuáHạn,
    ĐãTrả

}
