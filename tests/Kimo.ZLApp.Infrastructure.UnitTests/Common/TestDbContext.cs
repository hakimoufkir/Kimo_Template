using Microsoft.EntityFrameworkCore;
using Kimo.ZLApp.Domain.Locations;
using Kimo.ZLApp.Domain.WeatherForecasts;
using Kimo.ZLApp.Infrastructure.Common.Database;
using Kimo.ZLApp.Infrastructure.Locations;
using Kimo.ZLApp.Infrastructure.WeatherForecasts;

namespace Kimo.ZLApp.Infrastructure.UnitTests.Common;

public class TestDbContext(DbContextOptions<TestDbContext> options) : DbContext(options), IApplicationDbContext
{
    public DbSet<TestEntity> TestEntities { get; init; }

    public DbSet<Forecast> Forecasts { get; init; }

    public DbSet<Location> Locations { get; init; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new LocationConfiguration());
        modelBuilder.ApplyConfiguration(new ForecastConfiguration());

        base.OnModelCreating(modelBuilder);
    }

    public static async Task<TestDbContext> Create()
    {
        var options = new DbContextOptionsBuilder<TestDbContext>()
            .UseSqlite("Filename=:memory:")
            .Options;

        var context = new TestDbContext(options);
        await context.Database.OpenConnectionAsync();
        await context.Database.EnsureCreatedAsync();

        return context;
    }
}
