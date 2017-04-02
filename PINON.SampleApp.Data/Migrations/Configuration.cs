using System;
using System.Data.Entity.Migrations;
using PINON.SampleApp.Data.Models;

namespace PINON.SampleApp.Data.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<AppDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(AppDbContext context)
        {
            context.Hospitals.AddOrUpdate(
              p => p.HospitalName,
              new Hospital { HospitalName = "Christus St.Vincent", CreatedBy = "Tolga K", ModifiedBy = "Tolga K", CreatedOn = DateTime.UtcNow, ModifiedOn = DateTime.UtcNow },
              new Hospital { HospitalName = "Mercy Hospital", CreatedBy = "Tolga K", ModifiedBy = "Tolga K", CreatedOn = DateTime.UtcNow, ModifiedOn = DateTime.UtcNow }
            );

        }
    }
}