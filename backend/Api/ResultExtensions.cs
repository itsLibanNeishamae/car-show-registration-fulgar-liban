using Application.Common;
using Microsoft.AspNetCore.Mvc;

namespace Api;

public static class ResultExtensions
{
    public static IResult ToHttpResult<T>(this Result<T> result, Func<T, IResult>? onSuccess = null)
    {
        if (result.IsSuccess)
            return onSuccess is null ? Results.Ok(result.Value) : onSuccess(result.Value!);

        var error = result.Error!;
        var problem = new ProblemDetails
        {
            Title = error.Title,
            Detail = error.Detail,
            Status = error.Kind switch
            {
                ErrorKind.Validation => StatusCodes.Status400BadRequest,
                ErrorKind.NotFound => StatusCodes.Status404NotFound,
                ErrorKind.Conflict => StatusCodes.Status409Conflict,
                _ => StatusCodes.Status500InternalServerError
            }
        };

        if (error.Errors is not null)
            problem.Extensions["errors"] = error.Errors;

        return Results.Problem(problem);
    }
}
