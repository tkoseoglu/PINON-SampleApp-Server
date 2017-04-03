using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Http;

namespace PINON.SampleApp.WebApi.Helpers
{
    public class AppBaseController : ApiController
    {
        public bool CurrentUserIsAdmin()
        {
            var identity = (ClaimsIdentity)HttpContext.Current.User.Identity;
            var claims = identity.Claims;
            return claims.Any(p => p.Type == "http://schemas.microsoft.com/ws/2008/06/identity/claims/role" && p.Value == "Admin");
        }

        public string CurrentUserName()
        {
            return this.GetUserClaim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name");
        }

        public string GetUserClaim(string claimType)
        {
            var identity = (ClaimsIdentity)HttpContext.Current.User.Identity;
            var claims = identity.Claims;
            return claims.FirstOrDefault(p => p.Type == claimType)?.Value;
        }
    }
}