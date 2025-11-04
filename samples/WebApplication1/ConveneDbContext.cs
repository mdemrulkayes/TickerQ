using Microsoft.EntityFrameworkCore;
using TickerQ.EntityFrameworkCore.DbContextFactory;

namespace WebApplication1;

public class ConveneDbContext(DbContextOptions<ConveneDbContext> options) : DbContext(options)
{
}
