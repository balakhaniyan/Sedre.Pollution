﻿using BuildingBlocks.Infrastructure.Implementations;
using Microsoft.EntityFrameworkCore;
using Sedre.Pollution.Domain.Models;

namespace Sedre.Pollution.Infrastructure
{
    public class PollutionDbContext : DbContextBase
    {
        public DbSet<Indicator> Indicators { get; set; }
        public DbSet<DayIndicator> DayIndicators { get; set; }
        
        public PollutionDbContext(DbContextOptions options) :base(options)
        {
        }
        
    }
}