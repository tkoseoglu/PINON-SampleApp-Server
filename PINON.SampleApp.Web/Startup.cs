using Microsoft.Owin;
using Owin;
using PINON.SampleApp.Web;

[assembly: OwinStartup(typeof(Startup))]

namespace PINON.SampleApp.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);          
        }        
    }
}