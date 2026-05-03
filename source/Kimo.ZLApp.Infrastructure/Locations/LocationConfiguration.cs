using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Kimo.ZLApp.Domain.Locations;

namespace Kimo.ZLApp.Infrastructure.Locations;

[ExcludeFromCodeCoverage]
public class LocationConfiguration : IEntityTypeConfiguration<Location>
{
    public void Configure(EntityTypeBuilder<Location> builder)
    {
        builder.HasKey(l => l.Id);
        builder.Property(l => l.Id).ValueGeneratedOnAdd();

        builder.Property(l => l.Name).IsRequired().HasMaxLength(Location.NameMaxLength);
    }
}
