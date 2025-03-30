namespace ServiCar.Domain.Generics
{
    public class Result<TData, TError>
    {
        public TData Data { get; }
        public TError Error { get; }
        public bool IsSuccess { get; }

        private Result(TData data, TError error, bool isSuccess)
        {
            Data = data;
            Error = error;
            IsSuccess = isSuccess;
        }

        public static Result<TData, TError> Success(TData data) => new(data, default, true);
        public static Result<TData, TError> Fail(TError error) => new(default, error, false);
    }

}
