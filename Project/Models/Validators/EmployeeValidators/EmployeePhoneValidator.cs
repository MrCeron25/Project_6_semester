using Project.Models.Validators.Base;

namespace Project.Models.Validators
{
    internal class EmployeePhoneValidator : BaseErrorValidator<string>
    {
        private const int PhoneLength = 11;

        private bool IsNumeric(string data)
        {
            return long.TryParse(data, out _);
        }

        protected override bool ValidityCondition(string data) => data != null && !string.IsNullOrEmpty(data.Trim()) && data.Length == 11 && IsNumeric(data);

        public override bool IsValid(string data, out string error)
        {
            error = string.Empty;
            bool result = ValidityCondition(data);
            if (!result) error = $"Телефон не должен быть пустым и должен иметь только {PhoneLength} цифр";
            return result;
        }
    }
}
