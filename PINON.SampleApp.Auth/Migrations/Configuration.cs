using System.Data.Entity.Migrations;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using PINON.SampleApp.Auth.Models;

namespace PINON.SampleApp.Auth.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<AppAuthDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(AppAuthDbContext context)
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
            
            //var admin1 = userManager.FindByEmail("tolga.k@outlook.com");
            //if (admin1 != null) return;
            //admin1 = new UserAccount
            //{
            //    UserName = "tolga.k@outlook.com",
            //    Email = "tolga.k@outlook.com",
            //    LastName = "Koseoglu",
            //    FirstName = "Tolga",
            //    EmailConfirmed = true,
            //    PhoneNumber = "949.243.6632",
            //    PhoneNumberConfirmed = true
            //};

            //var userRole = new IdentityUserRole();
            //var role = roleManager.FindByName("Admin");
            //userRole.RoleId = role.Id;
            //userRole.UserId = admin1.Id;
            //admin1.Roles.Add(userRole);

            //userManager.Create(admin1, "Ktk19766");

            //patient

            var patient1 = userManager.FindByEmail("patient1@pinon.com");
            if (patient1 != null) return;
            patient1 = new UserAccount
            {
                UserName = "patient1@pinon.com",
                Email = "patient1@pinon.com",
                LastName = "One",
                FirstName = "Patient",
                EmailConfirmed = true,
                PhoneNumber = "949.243.6632",
                PhoneNumberConfirmed = true
            };
            var patient1Role = new IdentityUserRole();
            var patientRole = roleManager.FindByName("Patient");
            patient1Role.RoleId = patientRole.Id;
            patient1Role.UserId = patient1.Id;
            patient1.Roles.Add(patient1Role);

            userManager.Create(patient1, "Pat119766");
        }
    }
}