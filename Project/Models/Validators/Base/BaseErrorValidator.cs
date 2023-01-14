namespace Project.Models.Validators.Base
{
    internal abstract class BaseErrorValidator<T> : BaseValidator<T>
    {
        public abstract bool IsValid(T data, out string error);
        public bool IsNotValid(T data, out string error) => !IsValid(data, out error);
    }
}
