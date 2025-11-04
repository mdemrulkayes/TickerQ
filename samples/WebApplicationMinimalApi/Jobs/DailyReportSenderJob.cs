using TickerQ.Utilities.Base;

namespace WebApplicationMinimalApi.Jobs;

public class DailyReportSenderJob
{
    [TickerFunction("DailyReportSender", "*/5 * * * *")]
    public Task DailyReportSender(TickerFunctionContext<object> data) 
    {
        Console.WriteLine("Daily report sent at " + DateTime.UtcNow);
        return Task.CompletedTask;
    }

    [TickerFunction("TrowException", "*/10 * * * *")]
    public Task TrowException(TickerFunctionContext<object> data)
    {
        throw new Exception("This is a test exception thrown at " + DateTime.UtcNow);
    }

    [TickerFunction("GetWeatherForecastTicker", "*/1 * * * *")]
    public Task GetWeatherForecastTicker(TickerFunctionContext<object> data)
    {
        Console.WriteLine("GetWeatherForecastTicker executed at " + DateTime.UtcNow);
        return Task.CompletedTask;
    }
}
