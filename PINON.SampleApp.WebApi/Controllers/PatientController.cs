using System.Web.Http;
using PINON.SampleApp.WebApi.Filters;

namespace PINON.SampleApp.WebApi.Controllers
{
    public class PatientController : ApiController
    {
        [JwtAuthentication]
        public string Get()
        {
            return "Patient";
        }
    }
}