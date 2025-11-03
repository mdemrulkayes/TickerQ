using Microsoft.EntityFrameworkCore;
using TickerQ.Dashboard.DependencyInjection;
using TickerQ.DependencyInjection;
using TickerQ.EntityFrameworkCore.DependencyInjection;
using TickerQ.Utilities.Entities;
using WebApplication1;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ConveneDbContext>(opt =>
{
    opt.UseSqlServer(connectionString: builder.Configuration.GetConnectionString("ConveneDbContext"));
});

builder.Services.AddTickerQ(opt =>
{
    opt.ConfigureScheduler(config =>
    {
        config.MaxConcurrency = 5;
    });

    opt.AddOperationalStore(dbConfig => {
        //dbConfig.UseApplicationDbContext<ConveneDbContext>(TickerQ.EntityFrameworkCore.Customizer.ConfigurationType.UseModelCustomizer);

        dbConfig.UseTickerQDbContext<ConveneDbContext>(optionAction =>
        {
            optionAction.UseSqlServer(builder.Configuration.GetConnectionString("ConveneDbContext"));
        });
        dbConfig.IgnoreSeedDefinedCronTickers();

        dbConfig.UseTickerSeeder(
            async timeTicker => await timeTicker.AddAsync(new TimeTickerEntity {
                Id = Guid.NewGuid(),
                Function = "CleanUpLogs",
                ExecutionTime = DateTime.Now.AddSeconds(30)
            })
            ,
            async cornSeeder =>
        {
            await cornSeeder.AddAsync(new CronTickerEntity
            {
                Id = Guid.NewGuid(),
                Function = "DailyReportSender",
                Expression = "*/5 * * * *"
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

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
