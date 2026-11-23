using Domain.Enums;
using FluentValidation;
using System.Globalization;

namespace Application.Feature.Registrations.GetRegistrations;

public sealed class Validator : AbstractValidator<Query>
{
    public Validator()
    {
        RuleFor(query => query.ParticipantName).MaximumLength(150);
        RuleFor(query => query.Page).Must(value =>
                int.TryParse(value, NumberStyles.None, CultureInfo.InvariantCulture, out var page) && page >= 1)
            .WithMessage("Page must be at least 1.");
        RuleFor(query => query.PageSize).Must(value =>
                int.TryParse(value, NumberStyles.None, CultureInfo.InvariantCulture, out var pageSize) &&
                pageSize is >= 1 and <= 50)
            .WithMessage("PageSize must be between 1 and 50.");
        RuleFor(query => query.Type).Must(value => IsValidEnum<RegistrationType>(value))
            .WithMessage("Type must be Exhibitor, Competitor, or VIP.");
        RuleFor(query => query.Status).Must(value => IsValidEnum<RegistrationStatus>(value))
            .WithMessage("Status must be Pending, Confirmed, CheckedIn, or Cancelled.");
    }

    private static bool IsValidEnum<TEnum>(string? value) where TEnum : struct, Enum =>
        string.IsNullOrWhiteSpace(value) ||
        (!int.TryParse(value, out _) && Enum.TryParse<TEnum>(value, true, out var parsed) && Enum.IsDefined(parsed));
}
