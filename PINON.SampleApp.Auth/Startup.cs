using Microsoft.Owin;
using Owin;
using PINON.SampleApp.Identity;

[assembly: OwinStartup(typeof(Startup))]

namespace PINON.SampleApp.Identity
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.CreatePerOwinContext(AppIdentityDbContext.Create);
            app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);
            app.CreatePerOwinContext<ApplicationSignInManager>(ApplicationSignInManager.Create);
        }
    }
}