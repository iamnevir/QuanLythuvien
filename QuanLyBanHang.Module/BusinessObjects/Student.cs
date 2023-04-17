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
[NavigationItem(Generate.User)]
[DefaultListViewOptions(MasterDetailMode.ListViewOnly, true, NewItemRowPosition.Top)]
[ListViewFindPanel(true)]
[ListViewAutoFilterRow(true)]
[LookupEditorMode(LookupEditorMode.AllItemsWithSearch)]
[XafDisplayName("Sinh Viên")]
[Appearance("StudentActive", AppearanceItemType = "ViewItem", TargetItems = "*",
    Criteria = "Active=false", Context = "ListView", BackColor = "Red",
        FontColor = "White", FontStyle = FontStyle.Strikeout, Priority = 2)]
[ImageName("BO_Customer")]
[XafDefaultProperty("Name")]
public class Student : BaseObject
{
    public Student(Session session)
        : base(session)
    {
    }
    public override void AfterConstruction()
    {
        base.AfterConstruction();
        Active = true;
    }
    bool active;
    string lastName;
    string middleName;
    string firstName;
    DateTime birthDay;
    string studentClass;
    string studentCode;
    [VisibleInDetailView(false)]
    public string Name
    {
        get => ObjectFormatter.Format("{FirstName} {MiddleName} {LastName}",this,EmptyEntriesMode.RemoveDelimiterWhenEntryIsEmpty);
    }
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
    [XafDisplayName("Mã sinh viên")]
    [RuleRequiredField("Bắt buộc phải điền Mã sinh viên",DefaultContexts.Save,"Trường không được để trống")]
    [RuleUniqueValue]
    public string StudentCode
    {
        get => studentCode;
        set => SetPropertyValue(nameof(StudentCode), ref studentCode, value);
    }

    [XafDisplayName("Lớp")]
    public string StudentClass
    {
        get => studentClass;
        set => SetPropertyValue(nameof(StudentClass), ref studentClass, value);
    }
    [ModelDefault("EditMask", "MMM/d/yyyy hh:mm tt")]
    [ModelDefault("DisplayFormat", "D")]
    [XafDisplayName("Ngày sinh")]
    public DateTime BirthDay
    {
        get => birthDay;
        set => SetPropertyValue(nameof(BirthDay), ref birthDay, value);
    }
    [XafDisplayName("Quyền Mượn")]
    [CaptionsForBoolValues("Có", "Không")]
    public bool Active
    {
        get => active;
        set => SetPropertyValue(nameof(Active), ref active, value);
    }
    [Association("Student-LoanCards")]
    public XPCollection<LoanCard> LoanCards
    {
        get
        {
            return GetCollection<LoanCard>(nameof(LoanCards));
        }
    }

}