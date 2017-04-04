using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;
using PINON.SampleApp.Common;
using PINON.SampleApp.Identity.Models;

namespace PINON.SampleApp.Identity
{
    public class AppIdentityDbContext : IdentityDbContext<UserAccount>
    {
        public AppIdentityDbContext()
            : base("DefaultConnection", false)
        {

            Database.Connection.ConnectionString = Constants.Db_ConnetionString;
        }

        public DbSet<UserAccountRole> UserAccountRoles { get; set; }

        public static AppIdentityDbContext Create()
        {
            return new AppIdentityDbContext();
        }
    }
}