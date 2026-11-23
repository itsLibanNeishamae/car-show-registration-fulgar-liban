using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class RegistrationsConfigurations : IEntityTypeConfiguration<Registration>
{
    public void Configure(EntityTypeBuilder<Registration> builder)
    {
        builder.ToTable("Registrations");
        builder.HasKey(registration => registration.Id);

        builder.Property(registration => registration.Id).ValueGeneratedNever();
        builder.Property(registration => registration.RegistrationNumber)
            .HasMaxLength(30).IsRequired();
        builder.HasIndex(registration => registration.RegistrationNumber).IsUnique();
        builder.Property(registration => registration.RegistrationDate).IsRequired();
        builder.Property(registration => registration.ParticipantName).HasMaxLength(150).IsRequired();
        builder.Property(registration => registration.ContactNumber).HasMaxLength(20).IsRequired();
        builder.Property(registration => registration.Email).HasMaxLength(200).IsRequired();
        builder.Property(registration => registration.Address).HasMaxLength(300).IsRequired();
        builder.Property(registration => registration.CarMakeModel).HasMaxLength(100).IsRequired();
        builder.Property(registration => registration.PlateNumber).HasMaxLength(15).IsRequired();
        builder.HasIndex(registration => registration.PlateNumber).IsUnique();
        builder.Property(registration => registration.RegistrationType).IsRequired();
        builder.Property(registration => registration.Status).IsRequired();
        builder.Property(registration => registration.RegistrationFee).HasPrecision(18, 2).IsRequired();
        builder.Property(registration => registration.AmountPaid).HasPrecision(18, 2).IsRequired();
        builder.Ignore(registration => registration.PaymentStatus);
    }
}
