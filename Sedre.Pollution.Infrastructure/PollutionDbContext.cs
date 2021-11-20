using BuildingBlocks.Infrastructure.Implementations;
using Microsoft.EntityFrameworkCore;
using Sedre.Pollution.Domain.Models;

namespace Sedre.Pollution.Infrastructure
{
    public class PollutionDbContext : DbContextBase
    {
        public DbSet<Indicator> Indicators { get; set; }
        
        public PollutionDbContext(DbContextOptions options) :base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Indicator>().HasKey(x => x.Id);
        }
    }
}