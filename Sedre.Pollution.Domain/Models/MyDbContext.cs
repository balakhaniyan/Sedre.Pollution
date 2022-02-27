using System.Data.Entity;

namespace Sedre.Pollution.Domain.Models
{
    public class MyDbContext: DbContext
    {
        public MyDbContext()
        {

        }
        
        public DbSet<DayIndicator> DayIndicators { get; set; } 
        public DbSet<MonthIndicator> MonthIndicators { get; set; }
        public DbSet<HourIndicator> HourIndicators { get; set; }
    }
}