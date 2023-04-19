using DevExpress.ExpressApp;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using System.ComponentModel;
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
    Criteria = "Status=2", Context = "Any", BackColor = "Green",
        FontColor = "White", FontStyle = FontStyle.Strikeout, Priority = 2)]
[Appearance("LoanCardObject1", AppearanceItemType = "ViewItem", TargetItems = "*",
    Criteria = "Status=1", Context = "Any", BackColor = "Red",
        FontColor = "White", Priority = 2)]
[Appearance("LoanCardObject3", AppearanceItemType = "ViewItem", TargetItems = "*",
    Criteria = "Active=true", Context = "Any", Enabled = false, Priority = 2)]
[ListViewFilter("ĐãTrả", "Status=2", ImageName = "PaymentPaid", Index = 0)]
[ListViewFilter("ChưaTrả", "Status=0", ImageName = "PaymentRefund", Index = 1)]
[ListViewFilter("QuáHạn", "Status=1", ImageName = "PaymentUnPaid", Index = 2)]
[DefaultProperty(nameof(LoanCard))]
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
        loanDate = DateTime.Now;
        createdDate = DateTime.Now;
    }
    [Persistent("ExpectedAmount")]
    decimal expectedAmount;
    [Persistent("CreatedDate")]
    DateTime createdDate;
    Employee employee;
    [Persistent("TotalLoanPrice")]
    decimal totalLoanPrice;
    decimal loanPrice;
    Student student;
    Status status;
    [Persistent("ActualPaymentDate")]
    DateTime actualPaymentDate;
    DateTime expectedPaymentDate;
    [Persistent("LoanDate")]
    DateTime loanDate;
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
    [RuleRequiredField]
    public DateTime LoanDate
    {
        get => loanDate;
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
        get
        {
            if (!IsLoading && !IsSaving)
                UpdateNgayTraThucTe(false);
            return actualPaymentDate;
        }
    }
    [PersistentAlias(nameof(createdDate))]
    [XafDisplayName("Ngày tạo thẻ mượn")]
    [RuleRequiredField]
    public DateTime CreatedDate
    {
        get => createdDate;
    }
    [XafDisplayName("Cho Thuê")]
    [ModelDefault("AllowEdit", "false")]
    public bool Active
    {
        get => GetPropertyValue<bool>(nameof(Active));
        set => SetPropertyValue(nameof(Active), value);
    }
    [XafDisplayName("Trạng Thái")]
    public Status Status
    {
        get
        {
            if (!IsLoading && !IsSaving && status==Status.QuáHạn)
                UpdateTrangThaiMuon(false);
            return status;
        }
        set => SetPropertyValue(nameof(Status), ref status, value);
    }

    [Association("Student-LoanCards")]
    [RuleRequiredField]
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
    [RuleRequiredField]
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
    public void UpdateNgayTraThucTe(bool forceChangeEvents)
    {
        DateTime? oldDate = actualPaymentDate;
        if (Status == Status.ĐãTrả)
        {
            actualPaymentDate = DateTime.Now;
        }
        if (forceChangeEvents)
            OnChanged(nameof(ActualPaymentDate), oldDate, actualPaymentDate);
    }
    public void UpdateTrangThaiMuon(bool forceChangeEvents)
    {
        Status? oldStatus = status;
        if (DateTime.Now>=ExpectedPaymentDate)
        {
            status = Status.QuáHạn;
        }
        if (forceChangeEvents)
            OnChanged(nameof(Status), oldStatus, status);
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
    [Action(ToolTip = "Điều chỉnh trạng thái mượn của thẻ", Caption = "Trả sách")]
    public void StatusChanged()
    {
        Status = Status.ĐãTrả;
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
