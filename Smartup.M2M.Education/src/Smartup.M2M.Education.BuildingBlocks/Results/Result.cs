
namespace Smartup.M2M.Education.BuildingBlocks.Results
{
    public class Result
    {
        public bool IsSuccess { get; }
        public bool IsFailure => !IsSuccess;
        public Error Error { get; }
        public IReadOnlyList<Error> Errors { get; }

        protected Result(bool isSuccess, Error error)
        {
            if (isSuccess && error != Error.None)
            {
                throw new ArgumentException("Success response cannot include an error", nameof(error));
            }

            if (!isSuccess && error == Error.None)
            {
                throw new ArgumentException("Failure response must include at least one error", nameof(error));
            }

            IsSuccess = isSuccess;
            Error = error;
            Errors = error == Error.None ? [] : [error];
        }

        protected Result(bool isSuccess, IReadOnlyList<Error> errors)
        {
            ArgumentNullException.ThrowIfNull(errors);

            if (isSuccess && errors.Count > 0)
            {
                throw new ArgumentException("Success response cannot include to error", nameof(errors));
            }

            if (!isSuccess && errors.Count == 0)
            {
                throw new ArgumentException("Failure response must include at least one error", nameof(errors));
            }

            IsSuccess = isSuccess;
            Error = errors.Count > 0 ? errors[0] : Error.None;
            Errors = errors;
        }

        public static Result Success() => new(true, Error.None);
        public static Result Failure(Error error) => new(false, error);
        public static Result Failure(IReadOnlyList<Error> errors) => new(false, errors);

        public static Result<TValue> Success<TValue>(TValue value) => new(value, true, Error.None);
        public static Result<TValue> Failure<TValue>(Error error) => new(default, false, error);
        public static Result<TValue> Failure<TValue>(IReadOnlyList<Error> errors) => new(default, false, errors);
    }

    public sealed class Result<TValue> : Result
    {
        private readonly TValue? _value;

        public TValue Value => IsSuccess
            ? _value!
            : throw new InvalidOperationException("Failure response cannot have a value");

        internal Result(TValue? value, bool isSuccess, Error error)
            : base(isSuccess, error)
        {
            _value = value;
        }

        internal Result(TValue? value, bool isSuccess, IReadOnlyList<Error> errors)
            : base(isSuccess, errors)
        {
            _value = value;
        }

        public static implicit operator Result<TValue>(TValue? value) =>
            value is not null ? Success(value) : Failure<TValue>(Error.NotFound("Common.NullValue", "Value can not be null"));
    }

}
