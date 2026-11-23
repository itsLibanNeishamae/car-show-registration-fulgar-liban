namespace Application.Feature.Registrations.GetRegistrationSummary;

public sealed record Response(
    long TotalRegistrations,
    long Confirmed,
    long CheckedIn,
    long Pending,
    long Cancelled,
    decimal TotalFees,
    decimal TotalPayments,
    decimal UncollectedPayments);
