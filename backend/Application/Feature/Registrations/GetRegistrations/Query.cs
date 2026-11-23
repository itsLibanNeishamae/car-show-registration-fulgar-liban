namespace Application.Feature.Registrations.GetRegistrations;

public sealed class Query
{
    public string? ParticipantName { get; set; }
    public string? Type { get; set; }
    public string? Status { get; set; }
    public string? Page { get; set; } = "1";
    public string? PageSize { get; set; } = "10";
}
