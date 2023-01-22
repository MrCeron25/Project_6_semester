using Project.Models.Validators.Base;

namespace Project.Models.Validators.PavilionValidators
{
    internal class PavilionValueAddedFactorValidator : BaseErrorValidator<double>
    {
        public const double MinValueAddedFactor = 0.1;
        protected override bool ValidityCondition(double data) => data >= MinValueAddedFactor;
        public override bool IsValid(double data, out string error)
        {
            error = string.Empty;
            bool result = ValidityCondition(data);
            if (!result) error = $"Коэффициент добавочной стоимости должен быть >= {MinValueAddedFactor}";
            return result;
        }
    }
}
