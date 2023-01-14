using Project.Models.Validators.Base;

namespace Project.Models.Validators.MallValidators
{
    internal class MallPhotoValidator : BaseErrorValidator<byte[]>
    {
        protected override bool ValidityCondition(byte[] data) => data != null;

        public override bool IsValid(byte[] data, out string error)
        {
            error = string.Empty;
            bool result = ValidityCondition(data);
            if (!result) error = "Картинка не должна быть пустой";
            return result;
        }
    }
}
