using Project.Models.Validators.Base;

namespace Project.Models.Validators
{
    internal class MallCostValidator : BaseErrorValidator<decimal>
    {
        const decimal MinCost = 0;
        protected override bool ValidityCondition(decimal data) => data >= MinCost;

        public override bool IsValid(decimal data, out string error)
        {
            error = string.Empty;
            bool result = ValidityCondition(data);
            if (!result) error = $"Цена должна быть >= {MinCost}";
            return result;
        }
    }
}
