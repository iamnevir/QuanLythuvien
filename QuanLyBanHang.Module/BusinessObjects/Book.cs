using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.BaseImpl.PermissionPolicy;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using System.ComponentModel;

namespace QuanLyBanHang.Module.BusinessObjects;

[DefaultClassOptions]
[DefaultProperty(nameof(Name))]
[DefaultListViewOptions(MasterDetailMode.ListViewOnly, true, NewItemRowPosition.Top)]
[ListViewFindPanel(true)]
[ListViewAutoFilterRow(true)]
[LookupEditorMode(LookupEditorMode.AllItemsWithSearch)]
[NavigationItem(Generate.Book)]
[XafDisplayName("Sách")]
[ImageName("Actions_Book")]
public class Book : BaseObject
{
    public Book(Session session)
        : base(session)
    {
    }
    public override void AfterConstruction()
    {
        base.AfterConstruction();
        ngayTao = DateTime.Now;
    }

    [Persistent("NgayTao")]
    DateTime ngayTao;
    decimal loanPrice;
    LoanCard loanCard;
    Category category;
    string publish;
    string author;
    [Persistent("CreatedBy")]
    readonly string createdBy = nameof(PermissionPolicyUser.UserName);
    byte[] image;
    DateTime createdDate;
    int quantity;
    decimal price;
    string description;
    string name;
    [XafDisplayName("Tên Sách")]
    [RuleRequiredField("Bắt buộc phải có Tên Sách", DefaultContexts.Save, "Trường dữ liệu không được để trống")]
    public string Name
    {
        get => name; set => SetPropertyValue(nameof(Name), ref name, value);
    }

    [XafDisplayName("Tác Giả")]
    [RuleRequiredField("Bắt buộc phải có Tác Giả", DefaultContexts.Save, "Trường dữ liệu không được để trống")]
    public string Author
    {
        get => author;
        set => SetPropertyValue(nameof(Author), ref author, value);
    }

    [XafDisplayName("Nhà xuất bản")]
    [RuleRequiredField("Bắt buộc phải có Nhà xuất bản", DefaultContexts.Save, "Trường dữ liệu không được để trống")]
    public string Publish
    {
        get => publish;
        set => SetPropertyValue(nameof(Publish), ref publish, value);
    }

    [XafDisplayName("Mô tả")]
    [Size(SizeAttribute.Unlimited)]
    [EditorAlias(EditorAliases.RichTextPropertyEditor)]
    public string Description
    {
        get => description;
        set => SetPropertyValue(nameof(Description), ref description, value);
    }
    [ModelDefault("DisplayFormat", "{0,-10:N0}Đ")]
    [XafDisplayName("Giá")]
    public decimal Price
    {
        get => price;
        set => SetPropertyValue(nameof(Price), ref price, value);
    }
    [ModelDefault("DisplayFormat", "{0,-10:N0}Đ")]
    [XafDisplayName("Giá mượn")]
    public decimal LoanPrice
    {
        get => loanPrice;
        set
        {
            bool modified = SetPropertyValue(nameof(LoanPrice), ref loanPrice, value);
            if (!IsLoading && !IsSaving && LoanCard != null && modified)
            {
                LoanCard.UpdateLoanPrice(true);
                LoanCard.UpdateTotalLoanPrice(true);
                LoanCard.UpdateExpectedAmount(true);
            }
        }
    }
    [XafDisplayName("Số lượng")]
    public int Quantity
    {
        get => quantity;
        set => SetPropertyValue(nameof(Quantity), ref quantity, value);
    }
    [ModelDefault("EditMask", "MMM/d/yyyy hh:mm tt")]
    [ModelDefault("DisplayFormat", "D")]
    [XafDisplayName("Ngày xuất bản")]
    public DateTime CreatedDate
    {
        get => createdDate;
        set => SetPropertyValue(nameof(CreatedDate), ref createdDate, value);
    }
    [ModelDefault("EditMask", "MMM/d/yyyy hh:mm tt")]
    [ModelDefault("DisplayFormat", "D")]
    [PersistentAlias(nameof(ngayTao))]
    [XafDisplayName("Ngày tạo")]
    [RuleRequiredField]
    public DateTime NgayTao
    {
        get => ngayTao;
    }
    [PersistentAlias(nameof(createdBy))]
    [XafDisplayName("Người Tạo")]
    public string CreatedBy
    {
        get => createdBy;
    }
    [XafDisplayName("Hình Ảnh")]
    [ImageEditor(ListViewImageEditorMode = ImageEditorMode.PopupPictureEdit,
    DetailViewImageEditorMode = ImageEditorMode.PictureEdit,
    ListViewImageEditorCustomHeight = 40)]
    public byte[] Image
    {
        get => image;
        set => SetPropertyValue(nameof(Image), ref image, value);
    }


    [Association("Category-Books")]
    public Category Category
    {
        get => category;
        set => SetPropertyValue(nameof(Category), ref category, value);
    }
    [VisibleInDetailView(false)]
    [VisibleInListView(false)]
    [Association("LoanCard-Books")]
    public LoanCard LoanCard
    {
        get => loanCard;
        set
        {
            LoanCard oldLoanCard = loanCard;
            bool modified = SetPropertyValue(nameof(LoanCard), ref loanCard, value);
            if (!IsLoading && !IsSaving && oldLoanCard != loanCard && modified)
            {
                oldLoanCard ??= loanCard;
                oldLoanCard.UpdateLoanPrice(true);
                oldLoanCard.UpdateTotalLoanPrice(true);
                oldLoanCard.UpdateExpectedAmount(true);
            }
        }
    }
    private FileData document;
    [XafDisplayName("Tệp Sách đính kèm")]
    [DevExpress.Xpo.Aggregated, ExpandObjectMembers(ExpandObjectMembers.Never)]
    [FileTypeFilter("DocumentFiles", 1, "*.txt", "*.doc")]
    public FileData Document
    {
        get { return document; }
        set { SetPropertyValue(nameof(Document), ref document, value); }
    }

}