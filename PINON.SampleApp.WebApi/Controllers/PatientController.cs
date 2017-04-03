using System.Web.Http;
using PINON.SampleApp.Tokens.Filters;

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