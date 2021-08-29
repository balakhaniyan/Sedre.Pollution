using System;
using Microsoft.EntityFrameworkCore;

namespace Sedre.Pollution.Infrastructure
{
    public class PollutionDbContext : DbContextBase
    {
        
        public PollutionDbContext(DbContextOptions options) :base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
        }
    }
}