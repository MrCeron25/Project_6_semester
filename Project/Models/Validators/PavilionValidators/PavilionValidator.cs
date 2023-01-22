using Project.Models.Validators.Base;

namespace Project.Models.Validators.PavilionValidators
{
    internal class PavilionValidator : BaseErrorValidator<Pavilion>
    {
        public override bool IsValid(Pavilion data, out string error)
        {
            bool pavilionCostPerSquareMeterValidator = new PavilionCostPerSquareMeterValidator().IsValid(data.cost_per_square_meter, out error);
            if (!pavilionCostPerSquareMeterValidator) return false;
            bool pavilionFloorValidator = new PavilionFloorValidator().IsValid(data.floor, out error);
            if (!pavilionFloorValidator) return false;
            bool pavilionPavilionNumberValidator = new PavilionPavilionNumberValidator().IsValid(data.pavilion_number, out error);
            if (!pavilionPavilionNumberValidator) return false;
            bool pavilionSquareValidator = new PavilionSquareValidator().IsValid(data.square, out error);
            if (!pavilionSquareValidator) return false;
            bool pavilionValueAddedFactorValidator = new PavilionValueAddedFactorValidator().IsValid(data.value_added_factor, out error);
            if (!pavilionValueAddedFactorValidator) return false;
            return pavilionCostPerSquareMeterValidator &&
                   pavilionFloorValidator &&
                   pavilionPavilionNumberValidator &&
                   pavilionSquareValidator &&
                   pavilionValueAddedFactorValidator;
        }

        protected override bool ValidityCondition(Pavilion data) => false;
    }
}
