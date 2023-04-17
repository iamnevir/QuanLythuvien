using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;

namespace QuanLyBanHang.Module.BusinessObjects;

[DefaultClassOptions]
[NavigationItem(Generate.Employee)]
[XafDisplayName("Nhân viên (Online)")]
public class OnlineEmployee : Employee
{
    public OnlineEmployee(Session session)
        : base(session)
    {
    }
    public override void AfterConstruction()
    {
        base.AfterConstruction();
       
    }

    string visaCode;
    [XafDisplayName("Mã thẻ visa")]
    [RuleRequiredField("Mã thẻ visa không được để trống", DefaultContexts.Save, "Không được để trống trường này!")]
    public string VisaCode
    {
        get => visaCode;
        set => SetPropertyValue(nameof(VisaCode), ref visaCode, value);
    }
}