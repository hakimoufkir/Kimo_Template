using Kimo.FX.AspNet.HealthChecks;
using Kimo.ZLApp.Infrastructure.Common.Database;
using Kimo.ZLApp.Web.Common.Composition;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureLogger(builder.Configuration)
    .ConfigureHangfire(builder.Configuration)
    .ConfigureServices(builder.Configuration, builder.Environment)
    .ConfigureCors()
    .ConfigureApi()
    .ConfigureSwagger()
    .UseHealthChecks(builder.Configuration);

builder.Logging.ConfigureLogger();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    await dbContext.Database.MigrateAsync();
}

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseHsts()
        .UseDefaultFiles()
        .UseStaticFiles();
}

app.ConfigureApi();
app.UseHttpsRedirection()
    .UseRouting()
    .ConfigureCors() 
    .ConfigureHangfire()
    .ConfigureSwagger(app.Services)
    .UseEndpoints(endpoints => endpoints.AddHealthChecks());

try
{
    Log.Information("Starting web host...");
    app.Run();

    return 0;
}
catch (Exception ex)
{
    Log.Fatal(ex, "Web host terminated unexpectedly");
    return 1;
}
finally
{
    Log.Verbose("Web host exited. Closing and flushing the logger...");
    Log.CloseAndFlush();
}
