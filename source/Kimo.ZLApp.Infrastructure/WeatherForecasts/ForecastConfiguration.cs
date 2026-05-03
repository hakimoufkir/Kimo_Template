using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Kimo.ZLApp.Domain.WeatherForecasts;

namespace Kimo.ZLApp.Infrastructure.WeatherForecasts;

[ExcludeFromCodeCoverage]
public class ForecastConfiguration : IEntityTypeConfiguration<Forecast>
{
    public void Configure(EntityTypeBuilder<Forecast> builder)
    {
        builder.HasKey(f => f.Id);
        builder.Property(f => f.Id).ValueGeneratedOnAdd();

        builder.Property(f => f.Date).IsRequired();

        builder.HasOne(f => f.Location)
            .WithMany()
            .HasForeignKey(f => f.LocationId);

        builder.HasIndex(f => f.Date);

        builder.OwnsMany(f => f.Precipitations, p =>
        {
            p.WithOwner().HasForeignKey("ForecastId");
            p.HasKey("ForecastId", nameof(PrecipitationEntry.Hour));
            p.Property(x => x.Hour)
                .ValueGeneratedNever();
        });

        builder.OwnsOne(f => f.TemperatureData, td =>
        {
            td.WithOwner().HasForeignKey("ForecastId");

            td.OwnsMany(t => t.Temperatures, t =>
            {
                t.WithOwner().HasForeignKey("ForecastId");
                t.HasKey("ForecastId", nameof(TemperatureDataEntry.Hour));
                t.Property(x => x.Hour)
                    .ValueGeneratedNever();
            });
        });
    }
}
