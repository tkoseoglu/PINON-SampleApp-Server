using System.Data.Entity.Migrations;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using PINON.SampleApp.Identity.Models;

namespace PINON.SampleApp.Identity.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<AppIdentityDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(AppIdentityDbContext context)
        {
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var userManager = new UserManager<UserAccount>(new UserStore<UserAccount>(context));

            if (!roleManager.RoleExists("Admin"))
            {
                var adminUserAccountRole = new UserAccountRole
                {
                    Name = "Admin",
                    IsDeletable = false
                };
                roleManager.Create(adminUserAccountRole);
            }
            if (!roleManager.RoleExists("Patient"))
            {
                var patientUserAccountRole = new UserAccountRole
                {
                    Name = "Patient",
                    IsDeletable = true
                };
                roleManager.Create(patientUserAccountRole);
            }

            var admin1 = userManager.FindByEmail("jharsh@pinon.com");
            if (admin1 != null) return;
            admin1 = new UserAccount
            {
                UserName = "jharsh@pinon.com",
                Email = "jharsh@pinon.com",
                LastName = "Harsh",
                FirstName = "John",
                EmailConfirmed = true,
                PhoneNumber = "949.243.6632",
                PhoneNumberConfirmed = true
            };

            var userRole = new IdentityUserRole();
            var role = roleManager.FindByName("Admin");
            userRole.RoleId = role.Id;
            userRole.UserId = admin1.Id;
            admin1.Roles.Add(userRole);

            userManager.Create(admin1, "123456");

            //patient

            var patient1 = userManager.FindByEmail("mfassbender@pinon.com");
            if (patient1 != null) return;
            patient1 = new UserAccount
            {
                UserName = "mfassbender@pinon.com",
                Email = "mfassbender@pinon.com",
                LastName = "Fassbender",
                FirstName = "Michael",
                EmailConfirmed = true,
                PhoneNumber = "949.243.6632",
                PhoneNumberConfirmed = true
            };
            var patient1Role = new IdentityUserRole();
            var patientRole = roleManager.FindByName("Patient");
            patient1Role.RoleId = patientRole.Id;
            patient1Role.UserId = patient1.Id;
            patient1.Roles.Add(patient1Role);

            userManager.Create(patient1, "123456");
        }
    }
}