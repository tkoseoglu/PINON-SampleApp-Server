using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Http;
using PINON.SampleApp.Tokens.Filters;
using PINON.SampleApp.WebApi.Models;

namespace PINON.SampleApp.WebApi.Controllers.api
{
    [JwtAuthentication]    
    public class UtilController : ApiController
    {
        [HttpGet]
        [Route("api/Util/GetMenu")]
        public List<MenuItem> GetMenu()
        {
            var tabs = GetMenuAction();
            return tabs;
        }

        private List<MenuItem> GetMenuAction()
        {
            var tabs = new List<MenuItem>();
           
            var identity = (ClaimsIdentity)HttpContext.Current.User.Identity;
            var claims = identity.Claims;
            var isAdmin = claims.Any(p => p.Type == "http://schemas.microsoft.com/ws/2008/06/identity/claims/role" && p.Value == "Admin");

            if (isAdmin)
            {
                tabs.Add(new MenuItem
                {
                    Name = "Manage Patients",
                    Route = "/patients"
                });
                tabs.Add(new MenuItem
                {
                    Name = "Hospitals",
                    Route = "/hospitals"
                });
            }
            else
            {
                tabs.Add(new MenuItem
                {
                    Name = "My Portal",
                    Route = "/patient"
                });               
            }
            
            return tabs;
        }
    }
}