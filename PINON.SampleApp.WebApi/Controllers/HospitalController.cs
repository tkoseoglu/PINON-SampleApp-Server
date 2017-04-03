using System.Web.Http;
using PINON.SampleApp.WebApi.Filters;

namespace PINON.SampleApp.WebApi.Controllers
{
    public class HospitalController : ApiController
    {
        [JwtAuthentication]
        public string Get()
        {
            return "Hospital";
        }
    }
}