using Domain.Enums;

namespace Application.Feature.Registrations.CreateRegistration;

public sealed record Request(
    string ParticipantName,
    string ContactNumber,
    string Email,
    string Address,
    string CarMakeModel,
    string PlateNumber,
    RegistrationType RegistrationType,
    decimal AmountPaid = 0m);
