namespace Application.Common;

public enum ErrorKind
{
    Validation,
    NotFound,
    Conflict
}

public sealed record ResultError(
    ErrorKind Kind,
    string Title,
    string Detail,
    IReadOnlyDictionary<string, string[]>? Errors = null);

public sealed record Result<T>(T? Value, ResultError? Error)
{
    public bool IsSuccess => Error is null;

    public static Result<T> Success(T value) => new(value, null);
    public static Result<T> Failure(ResultError error) => new(default, error);

    public static Result<T> Validation(IReadOnlyDictionary<string, string[]> errors) =>
        new(default, new ResultError(
            ErrorKind.Validation,
            "One or more validation errors occurred.",
            "One or more validation errors occurred.",
            errors));

    public static Result<T> NotFound(string title, string detail) =>
        new(default, new ResultError(ErrorKind.NotFound, title, detail));

    public static Result<T> Conflict(string title, string detail) =>
        new(default, new ResultError(ErrorKind.Conflict, title, detail));
}
