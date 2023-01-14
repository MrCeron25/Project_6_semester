namespace Project.Models.Validators.Base
{
    internal abstract class BaseValidator<T>
    {
        protected abstract bool ValidityCondition(T data);
    }
}
