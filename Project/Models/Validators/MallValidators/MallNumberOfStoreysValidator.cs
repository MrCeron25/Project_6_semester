using Project.Models.Validators.Base;

namespace Project.Models.Validators.MallValidators
{
    internal class MallNumberOfStoreysValidator : BaseErrorValidator<int>
    {
        const decimal MinNumberOfStoreys = 1;
        protected override bool ValidityCondition(int data) => data >= MinNumberOfStoreys;

        public override bool IsValid(int data, out string error)
        {
            error = string.Empty;
            bool result = ValidityCondition(data);
            if (!result) error = $"Этажность должна быть >= {MinNumberOfStoreys}";
            return result;
        }
    }
}
