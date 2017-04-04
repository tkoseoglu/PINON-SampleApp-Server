using System.Data.Entity;
using PINON.SampleApp.Common;
using PINON.SampleApp.Data.Contracts;
using PINON.SampleApp.Data.Models;

namespace PINON.SampleApp.Data
{
    public class AppDbContext : DbContext, IAppDbContext
    {
        public AppDbContext()
        {
            Database.Connection.ConnectionString = Constants.Db_ConnetionString;
        }

        public DbSet<Hospital> Hospitals { get; set; }
        public DbSet<Patient> Patients { get; set; }
    }
}