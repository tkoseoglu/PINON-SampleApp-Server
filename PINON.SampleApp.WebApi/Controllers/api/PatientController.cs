using System.Web.Http;
using PINON.SampleApp.Tokens.Filters;

namespace PINON.SampleApp.WebApi.Controllers.api
{
    [JwtAuthentication]    
    public class PatientController : ApiController
    {
        
        public string Get()
        {
            return "Patient";
        }
    }
}