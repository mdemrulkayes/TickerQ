using Microsoft.EntityFrameworkCore;
using TickerQ.Dashboard.DependencyInjection;
using TickerQ.DependencyInjection;
using TickerQ.EntityFrameworkCore.DependencyInjection;
using TickerQ.Utilities.Entities;
using TickerQ.Utilities.Interfaces.Managers;
using WebApplication1;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ApplicationDbContext>(opt =>
{
    opt.UseSqlServer(connectionString: builder.Configuration.GetConnectionString("ApplicationDbContext"));
});

builder.Services.AddTickerQ(opt =>
{
    opt.ConfigureScheduler(config =>
    {
        config.MaxConcurrency = 5;
    });

    opt.AddOperationalStore(dbConfig =>
    {
        //dbConfig.UseApplicationDbContext<ApplicationDbContext>(TickerQ.EntityFrameworkCore.Customizer.ConfigurationType.UseModelCustomizer);

        dbConfig.UseTickerQDbContext<ApplicationDbContext>(optionAction =>
        {
            optionAction.UseSqlServer(builder.Configuration.GetConnectionString("ApplicationDbContext"));
        });

        //dbConfig.IgnoreSeedDefinedCronTickers();

        dbConfig.UseTickerSeeder(async cornSeeder =>
        {
            await cornSeeder.AddAsync(new CronTickerEntity
            {
                Id = Guid.NewGuid(),
                Function = "DailyReportSender",
                Expression = "*/5 * * * *",
            });
            await cornSeeder.AddAsync(new CronTickerEntity
            {
                Id = Guid.NewGuid(),
                Function = "TrowException",
                Expression = "*/10 * * * *"
            });
        });

    });

    opt.AddDashboard(config =>
    {
        config.SetBasePath("/tickerq-dashboard");
        config.WithBasicAuth("admin", "admin");
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseTickerQ();
app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", async (ICronTickerManager<CronTickerEntity> cronTickerManager, ITimeTickerManager<TimeTickerEntity> timeTickerManager) =>
{
    var forecast =  Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();

    var timeTicker = await timeTickerManager.AddAsync(new TimeTickerEntity
    {
        Id =  Guid.NewGuid(),
        Function = "OneTimeWeatherForecastTicker",
        ExecutionTime = DateTime.UtcNow.AddMinutes(2),
        Retries = 2,
    });

    var result = await cronTickerManager.AddAsync(new CronTickerEntity 
    {
        Id = Guid.NewGuid(),
        Function = "GetWeatherForecastTicker",
        Expression = "*/1 * * * *",
        Retries = 3,
        RetryIntervals = [20, 40, 60],
    });

    return forecast;
})
.WithName("GetWeatherForecast")
.WithOpenApi();

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
