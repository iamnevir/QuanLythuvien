﻿using DevExpress.ExpressApp;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.BaseImpl.PermissionPolicy;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using QuanLyBanHang.Module.Extension;
using System.ComponentModel;

namespace QuanLyBanHang.Module.BusinessObjects;

[DefaultClassOptions]
[DefaultProperty(nameof(Name))]
[DefaultListViewOptions(MasterDetailMode.ListViewOnly, true, NewItemRowPosition.Top)]
[ListViewFindPanel(true)]
[ListViewAutoFilterRow(true)]
[LookupEditorMode(LookupEditorMode.AllItemsWithSearch)]
[CustomNestedListView(nameof(LoanCard),AllowLink =true,AllowUnlink =false,AllowDelete =false,AllowEdit =false)]
[NavigationItem(Generate.Book)]
[XafDisplayName("Sách")]
[Appearance("TrangThaiSach", AppearanceItemType = "ViewItem", TargetItems = "Name,TrangThai",
    Criteria = "TrangThai=1", Context = "Any", FontColor = "Red",FontStyle =System.Drawing.FontStyle.Bold, Priority = 1)]
[Appearance("TrangThaiSach2", AppearanceItemType = "ViewItem", TargetItems = "*",
    Criteria = "TrangThai=1", Context = "Any", Enabled = false, Priority = 1)]
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
    private FileData document;
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
    [Size(1024)]
    [ImageEditor(ListViewImageEditorCustomHeight = 100, DetailViewImageEditorFixedHeight = 80)]
    public byte[] Image
    {
        get => image;
        set => SetPropertyValue(nameof(Image), ref image, value);
    }

    [XafDisplayName("Tệp Sách đính kèm")]
    [DevExpress.Xpo.Aggregated, ExpandObjectMembers(ExpandObjectMembers.Never)]
    [FileTypeFilter("DocumentFiles", 1, "*.txt", "*.doc")]
    public FileData Document
    {
        get { return document; }
        set { SetPropertyValue(nameof(Document), ref document, value); }
    }
    [XafDisplayName("Trạng thái sách")]
    [ModelDefault("AllowEdit", "false")]
    public TrangThai TrangThai
    {
        get => GetPropertyValue<TrangThai>(nameof(TrangThai));
        set
        {
            if (Quantity > 0)
            {
                SetPropertyValue(nameof(TrangThai) ,TrangThai.conhang);
            }
            else
            {
                SetPropertyValue(nameof(TrangThai), TrangThai.hethang);
            }
        } 
    }
    public void UpdateSoluong(bool forceChangeEvents)
    {
        int? oldquantity = Quantity;
        if (LoanCard.Active == true)
        {           
            if (LoanCard.Status == Status.ĐãTrả)
            {
                quantity++;
            }
            if (loanCard.Status == Status.ChưaTrả || loanCard.Status == Status.QuáHạn)
            {
                quantity--;
            }
        }
       
        if (forceChangeEvents)
            OnChanged(nameof(Quantity), oldquantity, quantity);
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
                UpdateSoluong(true);
            }
        }
    }


}
public enum TrangThai
{
    [XafDisplayName("Còn hàng")]
    conhang,
    [XafDisplayName("Hết hàng")]
    hethang
}