using Project.Models.Validators.Base;

namespace Project.Models.Validators
{
    internal class MallNameValidator : BaseErrorValidator<string>
    {
        protected override bool ValidityCondition(string data) => data != null && !string.IsNullOrEmpty(data.Trim());

        public override bool IsValid(string data, out string error)
        {
            error = string.Empty;
            bool result = ValidityCondition(data);
            if (!result) error = "Название не должно быть пустым";
            return result;
        }
    }
}
