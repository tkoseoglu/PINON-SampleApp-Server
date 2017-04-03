using System.Collections.Generic;
using System.Web.Http;
using PINON.SampleApp.WebApi.Models;

namespace PINON.SampleApp.WebApi.Controllers
{
    public class UtilController : ApiController
    {
        [HttpGet]
        [AllowAnonymous]
        [Route("api/Util/GetMenu")]
        public List<MenuItem> GetMenu()
        {
            var tabs = GetMenuAction();
            return tabs;
        }

        private List<MenuItem> GetMenuAction()
        {
            var tabs = new List<MenuItem>
            {
                new MenuItem
                {
                    Name = "Home",
                    Route = "/home"
                },
                new MenuItem
                {
                    Name = "Patients",
                    Route = "/patients"
                },
                new MenuItem
                {
                    Name = "Hospitals",
                    Route = "/hospitals"
                }
            };


            //var userHasAdminRoleClaim = HttpContext.User.Claims.FirstOrDefault(p => p.Type == "Role" && p.Value == "Admin");
            //if (userHasAdminRoleClaim == null) return tabs;

            //tabs.Add(new MenuItem
            //{
            //    Name = "Administration",
            //    SubTabs = new List<MenuItem>
            //    {
            //        new MenuItem
            //        {
            //            Route = "users",
            //            Name = "Site Users"
            //        }
            //    }
            //});

            return tabs;
        }
    }
}