using FluentValidation;
using System.Text.RegularExpressions;

namespace Application.Feature.Registrations.CreateRegistration;

public sealed class Validator : AbstractValidator<Request>
{
    private static readonly Regex PhilippineMobilePattern =
        new(@"^09[0-9]{9}$", RegexOptions.Compiled | RegexOptions.CultureInvariant);

    public Validator()
    {
        RuleFor(request => request.ParticipantName)
            .Must(value => !string.IsNullOrWhiteSpace(value)).WithMessage("Participant name is required.")
            .MaximumLength(150);
        RuleFor(request => request.ContactNumber)
            .Must(value => !string.IsNullOrWhiteSpace(value)).WithMessage("Contact number is required.")
            .Matches(PhilippineMobilePattern).WithMessage("Contact number must match 09XXXXXXXXX.");
        RuleFor(request => request.Email)
            .Must(value => !string.IsNullOrWhiteSpace(value)).WithMessage("Email is required.")
            .EmailAddress().MaximumLength(200);
        RuleFor(request => request.Address)
            .Must(value => !string.IsNullOrWhiteSpace(value)).WithMessage("Address is required.")
            .MaximumLength(300);
        RuleFor(request => request.CarMakeModel)
            .Must(value => !string.IsNullOrWhiteSpace(value)).WithMessage("Car make and model is required.")
            .MaximumLength(100);
        RuleFor(request => request.PlateNumber)
            .Must(value => !string.IsNullOrWhiteSpace(value)).WithMessage("Plate number is required.")
            .MaximumLength(15);
        RuleFor(request => request.RegistrationType).IsInEnum();
        RuleFor(request => request.AmountPaid)
            .GreaterThanOrEqualTo(0)
            .LessThanOrEqualTo(request => FeeFor(request.RegistrationType))
            .WithMessage("Amount paid cannot exceed the fee for the selected registration type.");
    }

    private static decimal FeeFor(Domain.Enums.RegistrationType type) => type switch
    {
        Domain.Enums.RegistrationType.Exhibitor => 1500m,
        Domain.Enums.RegistrationType.Competitor => 2500m,
        Domain.Enums.RegistrationType.VIP => 5000m,
        _ => 0m
    };
}
