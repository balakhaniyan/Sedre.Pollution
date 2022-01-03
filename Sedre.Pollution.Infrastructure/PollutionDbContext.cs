using BuildingBlocks.Infrastructure.Implementations;
using Microsoft.EntityFrameworkCore;
using Sedre.Pollution.Domain.Models;

namespace Sedre.Pollution.Infrastructure
{
    public class PollutionDbContext : DbContextBase
    {
        public DbSet<HourIndicator> HourIndicators { get; set; }
        public DbSet<DayIndicator> DayIndicators { get; set; }
        public DbSet<MonthIndicator> MonthIndicators { get; set; }
        
        public PollutionDbContext(DbContextOptions options) :base(options)
        {
        }
        
    }
}