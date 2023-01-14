using Project.Models.Validators.Base;

namespace Project.Models.Validators
{
    internal class MallNumberOfPavilionValidator : BaseErrorValidator<int>
    {
        const decimal MinNumberOfPavilions = 0;
        protected override bool ValidityCondition(int data) => data >= MinNumberOfPavilions;

        public override bool IsValid(int data, out string error)
        {
            error = string.Empty;
            bool result = ValidityCondition(data);
            if (!result) error = $"Количество павильонов должно быть >= {MinNumberOfPavilions}";
            return result;
        }
    }
}
