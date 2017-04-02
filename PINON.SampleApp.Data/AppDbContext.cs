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
#if(RELEASE)
            Database.Connection.ConnectionString = @"Data Source=(local)\sqlexpress;Initial Catalog=TOLGA_Inx2;User ID=webapps;Password=elevated";
#elif(DEBUG)
            Database.Connection.ConnectionString = Constants.Debug_Db_ConnetionString;
#endif
        }

        public DbSet<Hospital> Hospitals { get; set; }
        public DbSet<Patient> Patients { get; set; }
    }
}