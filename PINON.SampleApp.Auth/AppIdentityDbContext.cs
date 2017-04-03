using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;
using PINON.SampleApp.Common;
using PINON.SampleApp.Identity.Contracts;
using PINON.SampleApp.Identity.Models;

namespace PINON.SampleApp.Identity
{
    public class AppIdentityDbContext : IdentityDbContext<UserAccount>
    {
        public AppIdentityDbContext()
            : base("DefaultConnection", false)
        {
#if(RELEASE)
            Database.Connection.ConnectionString = @"Data Source=(local)\sqlexpress;Initial Catalog=TOLGA_Inx2;User ID=webapps;Password=elevated";
#elif(DEBUG)
            Database.Connection.ConnectionString = Constants.Debug_Db_ConnetionString;
#endif
        }

        public DbSet<UserAccountRole> UserAccountRoles { get; set; }

        public static AppIdentityDbContext Create()
        {
            return new AppIdentityDbContext();
        }
    }
}