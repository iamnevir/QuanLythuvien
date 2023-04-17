using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;

namespace QuanLyBanHang.Module.BusinessObjects;

[DefaultClassOptions]
[NavigationItem(Generate.Employee)]
[XafDisplayName("Nhân viên (Trực tiếp)")]
public class OfflineEmployee : Employee
{ 
    public OfflineEmployee(Session session)
        : base(session)
    {
    }
    public override void AfterConstruction()
    {
        base.AfterConstruction();
       
    }

    string employeeCard;
    [XafDisplayName("Mã thẻ nhân viên")]
    [RuleRequiredField("Mã thẻ nhân viên không được để trống", DefaultContexts.Save, "Không được để trống trường này!")]
    public string EmployeeCard
    {
        get => employeeCard;
        set => SetPropertyValue(nameof(EmployeeCard), ref employeeCard, value);
    }
}