using DevExpress.ExpressApp;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;

namespace QuanLyBanHang.Module.BusinessObjects;

[DefaultListViewOptions(MasterDetailMode.ListViewOnly, true, NewItemRowPosition.Top)]
[ListViewFindPanel(true)]
[ListViewAutoFilterRow(true)]
[LookupEditorMode(LookupEditorMode.AllItemsWithSearch)]
[XafDisplayName("Nhân viên")]
[ImageName("BO_Employee")]
[Appearance("ActiveRole", AppearanceItemType = "ViewItem",
    TargetItems = "*", Context = "Any", Criteria = "Active=false",
    FontColor = "Red", FontStyle = System.Drawing.FontStyle.Strikeout, Priority = 1)]
public class Employee : BaseObject
{
    public Employee(Session session)
        : base(session)
    {
    }
    public override void AfterConstruction()
    {
        base.AfterConstruction();
        Active = true;
    }

    bool active;
    string xa;
    string district;
    string zipCode;
    string street;
    string country;
    string city;
    string lastName;
    string middleName;
    string firstName;
    string phoneNumber;
    DateTime birthDay;
    [VisibleInListView(false)]
    [XafDisplayName("Tên")]
    public string LastName
    {
        get => lastName;
        set => SetPropertyValue(nameof(LastName), ref lastName, value);
    }
    [VisibleInListView(false)]
    [XafDisplayName("Tên Đệm")]
    public string MiddleName
    {
        get => middleName;
        set => SetPropertyValue(nameof(MiddleName), ref middleName, value);
    }
    [VisibleInListView(false)]
    [XafDisplayName("Họ")]
    public string FirstName
    {
        get => firstName;
        set => SetPropertyValue(nameof(FirstName), ref firstName, value);
    }
    [VisibleInDetailView(false)]
    public string Name
    {
        get => ObjectFormatter.Format("{FirstName} {MiddleName} {LastName}", this, EmptyEntriesMode.RemoveDelimiterWhenEntryIsEmpty);
    }
    [ModelDefault("EditMask", "MMM/d/yyyy hh:mm tt")]
    [ModelDefault("DisplayFormat", "D")]
    [XafDisplayName("Ngày sinh")]
    public DateTime BirthDay
    {
        get => birthDay;
        set => SetPropertyValue(nameof(BirthDay), ref birthDay, value);
    }
    [RuleRequiredField("Số điện thoại không được để trống",DefaultContexts.Save,"Trường bắt buộc!")]
    [RuleUniqueValue]
    [ModelDefault("EditMask", "(+00) 0000 000 000")]
    [XafDisplayName("Số điện thoại")]
    public string PhoneNumber
    {
        get => phoneNumber;
        set => SetPropertyValue(nameof(PhoneNumber), ref phoneNumber, value);
    }
    [Association("Employee-LoanCards")]
    public XPCollection<LoanCard> LoanCards
    {
        get
        {
            return GetCollection<LoanCard>(nameof(LoanCards));
        }
    }
    [VisibleInDetailView(false)]
    public string Address
    {
        get => ObjectFormatter.Format("{Street},{Xa},{District},{City},{Country};{ZipCode}", this, EmptyEntriesMode.RemoveDelimiterWhenEntryIsEmpty);
    }
    [VisibleInListView(false)]
    [XafDisplayName("Tỉnh Thành Phố")]
    public string City
    {
        get => city;
        set => SetPropertyValue(nameof(City), ref city, value);
    }
    [VisibleInListView(false)]
    [XafDisplayName("Quốc gia")]
    public string Country
    {
        get => country;
        set => SetPropertyValue(nameof(Country), ref country, value);
    }
    [VisibleInListView(false)]
    [XafDisplayName("Đường-Số nhà")]
    public string Street
    {
        get => street;
        set => SetPropertyValue(nameof(Street), ref street, value);
    }

    public string Xa
    {
        get => xa;
        set => SetPropertyValue(nameof(Xa), ref xa, value);
    }
    [VisibleInListView(false)]
    [XafDisplayName("Quận Huyện")]
    public string District
    {
        get => district;
        set => SetPropertyValue(nameof(District), ref district, value);
    }
    [VisibleInListView(false)]
    [XafDisplayName("Mã Zip")]
    public string ZipCode
    {
        get => zipCode;
        set => SetPropertyValue(nameof(ZipCode), ref zipCode, value);
    }
    [XafDisplayName("Kích hoạt quyền")]
    [CaptionsForBoolValues("Có", "Không")]
    public bool Active
    {
        get => active;
        set => SetPropertyValue(nameof(Active), ref active, value);
    }
}
