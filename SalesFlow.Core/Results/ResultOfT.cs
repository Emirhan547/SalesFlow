namespace SalesFlow.Core.Results;

public class Result
{
    public bool IsSuccess { get; }

    public string Message { get; }

    public IReadOnlyList<string>? Errors { get; }

    protected Result( bool isSuccess, string message,IReadOnlyList<string>? errors = null)
    {
        IsSuccess = isSuccess;
        Message = message;
        Errors = errors;
    }

    public static Result Success(string message = "Operation completed successfully.")=> new(true, message);
    public static Result Failure(string message,IReadOnlyList<string>? errors = null) => new(false, message, errors);
}