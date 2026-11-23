using FluentValidation;
using Application.Common;
using Api;

namespace Api.Filters;

public sealed class ValidationFilter<T>(IValidator<T> validator) : IEndpointFilter where T : notnull
{
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        var model = context.Arguments.OfType<T>().FirstOrDefault();
        if (model is null)
            throw new InvalidOperationException($"No {typeof(T).Name} argument was supplied to the validation filter.");

        var result = await validator.ValidateAsync(model, context.HttpContext.RequestAborted);
        if (!result.IsValid)
            return Result<T>.Validation(new Dictionary<string, string[]>(result.ToDictionary()))
                .ToHttpResult(_ => Results.Ok());

        return await next(context);
    }
}
