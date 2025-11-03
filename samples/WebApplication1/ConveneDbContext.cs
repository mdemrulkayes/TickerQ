using Microsoft.EntityFrameworkCore;
using TickerQ.EntityFrameworkCore.DbContextFactory;

namespace WebApplication1;

public class ConveneDbContext(DbContextOptions<TickerQDbContext> options) : TickerQDbContext(options)
{
}
