using Domain.Entities;
using Domain.Enums;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

#nullable disable

namespace Infrastructure.Persistence.Migrations;

[DbContext(typeof(AppDbContext))]
partial class AppDbContextModelSnapshot : ModelSnapshot
{
    protected override void BuildModel(ModelBuilder modelBuilder)
    {
        modelBuilder.HasAnnotation("ProductVersion", "10.0.15");
        modelBuilder.Entity<Registration>(entity =>
        {
            entity.ToTable("Registrations");
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.PlateNumber).IsUnique();
            entity.HasIndex(e => e.RegistrationNumber).IsUnique();
            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.RegistrationNumber).HasMaxLength(30).IsRequired();
            entity.Property(e => e.RegistrationDate).IsRequired();
            entity.Property(e => e.ParticipantName).HasMaxLength(150).IsRequired();
            entity.Property(e => e.ContactNumber).HasMaxLength(20).IsRequired();
            entity.Property(e => e.Email).HasMaxLength(200).IsRequired();
            entity.Property(e => e.Address).HasMaxLength(300).IsRequired();
            entity.Property(e => e.CarMakeModel).HasMaxLength(100).IsRequired();
            entity.Property(e => e.PlateNumber).HasMaxLength(15).IsRequired();
            entity.Property(e => e.RegistrationType).HasConversion<int>().IsRequired();
            entity.Property(e => e.Status).HasConversion<int>().IsRequired();
            entity.Property(e => e.RegistrationFee).HasPrecision(18, 2).IsRequired();
            entity.Property(e => e.AmountPaid).HasPrecision(18, 2).IsRequired();
            entity.Ignore(e => e.PaymentStatus);
        });
        modelBuilder.Entity<RegistrationSequence>(entity =>
        {
            entity.ToTable("RegistrationSequences");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.LastNumber).IsRequired();
            entity.HasData(new { Id = 1, LastNumber = 0L });
        });
    }
}
