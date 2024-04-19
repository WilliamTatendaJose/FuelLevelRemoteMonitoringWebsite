using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using DeanRemoteMonitoringWeb.Server.Models.RAZDEN;

namespace DeanRemoteMonitoringWeb.Server.Data
{
    public partial class RAZDENContext : DbContext
    {
        public RAZDENContext()
        {
        }

        public RAZDENContext(DbContextOptions<RAZDENContext> options) : base(options)
        {
        }

        partial void OnModelBuilding(ModelBuilder builder);

       

        public DbSet<DeanRemoteMonitoringWeb.Server.Models.RAZDEN.FuelRefilling> FuelRefillings { get; set; }

        public DbSet<DeanRemoteMonitoringWeb.Server.Models.RAZDEN.FuelTank> FuelTanks { get; set; }

        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            configurationBuilder.Conventions.Add(_ => new BlankTriggerAddingConvention());
        }
    
    }
}