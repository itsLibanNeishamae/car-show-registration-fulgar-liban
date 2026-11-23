using System.Net.Mail;
using System.Text.RegularExpressions;
using Domain.Enums;

namespace Domain.Entities;

public class Registration
{
    private static readonly Regex PhilippineMobilePattern =
        new(@"^(?:\+63|63|0)9\d{9}$", RegexOptions.Compiled | RegexOptions.CultureInvariant);

    private Registration() { }

    private Registration(
        string registrationNumber,
        string participantName,
        string contactNumber,
        string email,
        string address,
        string carMakeModel,
        string plateNumber,
        RegistrationType registrationType)
    {
        Id = Guid.NewGuid();
        RegistrationNumber = Required(registrationNumber, nameof(RegistrationNumber), 20);
        RegistrationDate = DateTime.UtcNow;
        ParticipantName = Required(participantName, nameof(ParticipantName), 150);
        ContactNumber = Required(contactNumber, nameof(ContactNumber), 20);
        if (!PhilippineMobilePattern.IsMatch(ContactNumber))
            throw new ArgumentException("Contact number must be a valid Philippine mobile number.", nameof(contactNumber));
        Email = Required(email, nameof(Email), 200);
        try { _ = new MailAddress(Email); }
        catch (FormatException) { throw new ArgumentException("Email must be valid.", nameof(email)); }
        Address = Required(address, nameof(Address), 300);
        CarMakeModel = Required(carMakeModel, nameof(CarMakeModel), 100);
        PlateNumber = Required(plateNumber, nameof(PlateNumber), 15).ToUpperInvariant();
        if (!Enum.IsDefined(registrationType))
            throw new ArgumentOutOfRangeException(nameof(registrationType));
        RegistrationType = registrationType;
        Status = RegistrationStatus.Pending;
        RegistrationFee = registrationType switch
        {
            RegistrationType.Exhibitor => 1500m,
            RegistrationType.Competitor => 2500m,
            RegistrationType.VIP => 5000m,
            _ => throw new ArgumentOutOfRangeException(nameof(registrationType))
        };
        AmountPaid = 0m;
    }

    public Guid Id { get; private set; }
    public string RegistrationNumber { get; private set; } = string.Empty;
    public DateTime RegistrationDate { get; private set; }
    public string ParticipantName { get; private set; } = string.Empty;
    public string ContactNumber { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public string Address { get; private set; } = string.Empty;
    public string CarMakeModel { get; private set; } = string.Empty;
    public string PlateNumber { get; private set; } = string.Empty;
    public RegistrationType RegistrationType { get; private set; }
    public RegistrationStatus Status { get; private set; }
    public decimal RegistrationFee { get; private set; }
    public decimal AmountPaid { get; private set; }
    public PaymentStatus PaymentStatus => AmountPaid == 0m
        ? PaymentStatus.Unpaid
        : AmountPaid == RegistrationFee ? PaymentStatus.Paid : PaymentStatus.Partial;

    // The server resolves the fee for the selected registration type before creating the entity.
    public static Registration Create(
        string registrationNumber,
        string participantName,
        string contactNumber,
        string email,
        string address,
        string carMakeModel,
        string plateNumber,
        RegistrationType registrationType) =>
        new(registrationNumber, participantName, contactNumber, email, address, carMakeModel, plateNumber, registrationType);

    public void RecordPayment(decimal amount)
    {
        if (amount < 0 || AmountPaid + amount > RegistrationFee)
            throw new ArgumentOutOfRangeException(nameof(amount), "Payment must keep the total between zero and the registration fee.");
        AmountPaid += amount;
    }

    public void Confirm()
    {
        if (Status != RegistrationStatus.Pending)
            throw new InvalidOperationException("Only pending registrations can be confirmed.");
        Status = RegistrationStatus.Confirmed;
    }

    public void CheckIn()
    {
        if (Status != RegistrationStatus.Confirmed)
            throw new InvalidOperationException("Only confirmed registrations can be checked in.");
        Status = RegistrationStatus.CheckedIn;
    }

    public void Cancel()
    {
        if (Status is RegistrationStatus.CheckedIn or RegistrationStatus.Cancelled)
            throw new InvalidOperationException("This registration cannot be cancelled.");
        Status = RegistrationStatus.Cancelled;
    }

    private static string Required(string value, string property, int maxLength)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException($"{property} is required.", property);
        var normalized = value.Trim();
        if (normalized.Length > maxLength)
            throw new ArgumentException($"{property} cannot exceed {maxLength} characters.", property);
        return normalized;
    }
}
