using Microsoft.EntityFrameworkCore;
using TickerQ.EntityFrameworkCore.DbContextFactory;

namespace WebApplication1;

//public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
//{
//}

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : TickerQDbContext<ApplicationDbContext, ExtendedTimeTickerEntity, ExtendedCronTickerEntity>(options)
{
    public DbSet<HelloEntity> Hellos { get; set; }
}


//public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : TickerQDbContext<ExtendedTimeTickerEntity, ExtendedCronTickerEntity>(options)
//{
//    public DbSet<HelloEntity> Hellos { get; set; }
//}


public class HelloEntity
{
    public int Id { get; set; }
    public string Name { get; set; }
}


public class ExtendedTimeTickerEntity : TickerQ.Utilities.Entities.TimeTickerEntity<ExtendedTimeTickerEntity>
{
    public string ExtendedTickerProperty { get; internal set; }
}


public class ExtendedCronTickerEntity : TickerQ.Utilities.Entities.CronTickerEntity
{
    public string ExtendedCronProperty { get; internal set; }
}