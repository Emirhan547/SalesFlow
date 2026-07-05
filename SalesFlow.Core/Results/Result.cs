namespace SalesFlow.Core.Results;

public class Result<T> : Result
{
    public T? Data { get; }

    private Result(bool isSuccess,T? data, string message, IReadOnlyList<string>? errors = null): base(isSuccess, message, errors)
    {
        Data = data;
    }

    public static Result<T> Success(T data, string message = "Operation completed successfully.") => new(true, data, message);

    public static new Result<T> Failure(string message,IReadOnlyList<string>? errors = null) => new(false, default, message, errors);
}