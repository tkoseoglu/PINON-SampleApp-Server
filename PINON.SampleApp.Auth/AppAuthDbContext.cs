using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;
using PINON.SampleApp.Auth.Models;
using PINON.SampleApp.Common;

namespace PINON.SampleApp.Auth
{
    public class AppAuthDbContext : IdentityDbContext<UserAccount>
    {
        public AppAuthDbContext()
            : base("DefaultConnection", false)
        {
#if(RELEASE)
            Database.Connection.ConnectionString = @"Data Source=(local)\sqlexpress;Initial Catalog=TOLGA_Inx2;User ID=webapps;Password=elevated";
#elif(DEBUG)
            Database.Connection.ConnectionString = Constants.Debug_Db_ConnetionString;
#endif
        }

        public DbSet<UserAccountRole> UserAccountRoles { get; set; }

        public static AppAuthDbContext Create()
        {
            return new AppAuthDbContext();
        }
    }
}