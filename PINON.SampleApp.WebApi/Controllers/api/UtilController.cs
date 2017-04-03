using System.Collections.Generic;
using System.Web.Http;
using PINON.SampleApp.Common;
using PINON.SampleApp.Tokens.Filters;
using PINON.SampleApp.WebApi.Helpers;

namespace PINON.SampleApp.WebApi.Controllers.api
{
    [JwtAuthentication]
    public class UtilController : AppBaseController
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

            if (CurrentUserIsAdmin())
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
                    Name = "Patient Portal",
                    Route = "/patient"
                });
            }

            return tabs;
        }
    }
}