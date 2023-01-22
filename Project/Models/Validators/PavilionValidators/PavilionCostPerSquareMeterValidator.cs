using Project.Models.Validators.Base;

namespace Project.Models.Validators.PavilionValidators
{
    internal class PavilionCostPerSquareMeterValidator : BaseErrorValidator<decimal>
    {
        public const decimal MinCostPerSquareMeter = 1000;
        protected override bool ValidityCondition(decimal data) => data >= MinCostPerSquareMeter;

        public override bool IsValid(decimal data, out string error)
        {
            error = string.Empty;
            bool result = ValidityCondition(data);
            if (!result) error = $"Стоимость павильона должна быть >= {MinCostPerSquareMeter}";
            return result;
        }
    }
}
