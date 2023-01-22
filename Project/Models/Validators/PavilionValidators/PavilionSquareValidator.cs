using Project.Models.Validators.Base;

namespace Project.Models.Validators.PavilionValidators
{
    internal class PavilionSquareValidator : BaseErrorValidator<decimal>
    {
        public const decimal MinSquare = 8;
        protected override bool ValidityCondition(decimal data) => data >= MinSquare;

        public override bool IsValid(decimal data, out string error)
        {
            error = string.Empty;
            bool result = ValidityCondition(data);
            if (!result) error = $"Площадь павильона должна быть >= {MinSquare}";
            return result;
        }
    }
}
