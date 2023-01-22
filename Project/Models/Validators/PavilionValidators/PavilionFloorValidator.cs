using Project.Models.Validators.Base;

namespace Project.Models.Validators.PavilionValidators
{
    internal class PavilionFloorValidator : BaseErrorValidator<int>
    {
       const int MinFloor = 0;
        protected override bool ValidityCondition(int data) => data >= MinFloor;

        public override bool IsValid(int data, out string error)
        {
            error = string.Empty;
            bool result = ValidityCondition(data);
            if (!result) error = $"Этаж должен быть >= {MinFloor}";
            return result;
        }
    }
}
