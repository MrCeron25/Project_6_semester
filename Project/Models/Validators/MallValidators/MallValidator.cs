using Project.Models.Validators.Base;

namespace Project.Models.Validators.MallValidators
{
    internal class MallValidator : BaseErrorValidator<Mall>
    {
        protected override bool ValidityCondition(Mall data) => false;
        public override bool IsValid(Mall data, out string error)
        {
            bool mallNameValidator = new MallNameValidator().IsValid(data.mall_name, out error);
            if (!mallNameValidator) return false;
            bool mallCostValidator = new MallCostValidator().IsValid(data.cost, out error);
            if (!mallCostValidator) return false;
            bool mallNumberOfPavilionValidator = new MallNumberOfPavilionValidator().IsValid(data.number_of_pavilion, out error);
            if (!mallNumberOfPavilionValidator) return false;
            bool mallCityValidator = new MallCityValidator().IsValid(data.city, out error);
            if (!mallCityValidator) return false;
            bool mallValueAddedFactorValidator = new PavilionValueAddedFactorValidator().IsValid(data.value_added_factor, out error);
            if (!mallValueAddedFactorValidator) return false;
            bool mallNumberOfStoreysValidator = new MallNumberOfStoreysValidator().IsValid(data.number_of_storeys, out error);
            if (!mallNumberOfStoreysValidator) return false;
            bool mallPhotoValidator = new MallPhotoValidator().IsValid(data.photo, out error);
            if (!mallPhotoValidator) return false;
            return mallNameValidator &&
                   mallCostValidator &&
                   mallNumberOfPavilionValidator &&
                   mallCityValidator &&
                   mallValueAddedFactorValidator &&
                   mallNumberOfStoreysValidator &&
                   mallPhotoValidator;
        }
    }
}
