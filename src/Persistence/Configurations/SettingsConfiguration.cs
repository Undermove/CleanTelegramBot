using Domain.New.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations;

public class SettingsConfiguration : IEntityTypeConfiguration<Settings>
{
    public void Configure(EntityTypeBuilder<Settings> builder)
    {
        builder.HasKey(e => e.UserId);
        builder.Property(e => e.UserId).HasColumnName("UserID");
        builder.Property(e => e.MeasuresTime).HasColumnType("datetime");
        builder.HasOne(d => d.User)
            .WithOne(p => p.Settings)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_Settings_Users");        
    }
}