using Project.Models.Validators.Base;

namespace Project.Models.Validators
{
    internal class EmployeeValidator : BaseErrorValidator<Employees>
    {
        protected override bool ValidityCondition(Employees data) => false;
        public override bool IsValid(Employees data, out string error)
        {
            bool employeeSurnameValidator = new EmployeeSurnameValidator().IsValid(data.surname, out error);
            if (!employeeSurnameValidator) return false;
            bool employeeNameValidator = new EmployeeNameValidator().IsValid(data.name, out error);
            if (!employeeNameValidator) return false;
            bool employeePatronymicValidator = new EmployeePatronymicValidator().IsValid(data.patronymic, out error);
            if (!employeePatronymicValidator) return false;
            bool employeeLoginValidator = new EmployeeLoginValidator().IsValid(data.login, out error);
            if (!employeeLoginValidator) return false;
            bool employeePasswordValidator = new EmployeePasswordValidator().IsValid(data.password, out error);
            if (!employeePasswordValidator) return false;
            bool employeePhoneValidator = new EmployeePhoneValidator().IsValid(data.phone, out error);
            if (!employeePhoneValidator) return false;
            return employeeSurnameValidator &&
                   employeeNameValidator &&
                   employeePatronymicValidator &&
                   employeeLoginValidator &&
                   employeePasswordValidator &&
                   employeePhoneValidator;
        }
    }
}
