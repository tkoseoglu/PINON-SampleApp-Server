using System.Threading.Tasks;
using System.Web.Mvc;
using PINON.SampleApp.Common;
using PINON.SampleApp.Identity.Contracts;
using PINON.SampleApp.WebApi.Helpers;
using PINON.SampleApp.WebApi.Models;

namespace PINON.SampleApp.WebApi.Controllers
{
    public class UserAccountController : Controller
    {
        private readonly IIdentityManager _identityManager;

        public UserAccountController(IIdentityManager identityManager)
        {
            _identityManager = identityManager;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> GetToken(LoginViewModel model)
        {
            var result = new TransactionResult();

            if (!ModelState.IsValid)
            {
                result.HasError = true;
                result.Message = "Missing login information";
                return Json(result, JsonRequestBehavior.AllowGet);
            }

            var identityLoginResult = await _identityManager.PasswordSignInAsync(model.Email, model.Password);
            if (identityLoginResult.HasError) return Json(identityLoginResult, JsonRequestBehavior.AllowGet);
            var jwtToken = JwtManager.GenerateToken(model.Email, false);
            var user = await _identityManager.FindByEmailAsync(model.Email);
            var response = new
            {
                access_token = jwtToken,                
                current_user = new
                {
                    user.FirstName,
                    user.LastName,
                    user.Email
                }
            };
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]        
        public ActionResult DestroyToken()
        {            
            var identityLoginResult = _identityManager.SignOut();                        
            return Json(identityLoginResult, JsonRequestBehavior.AllowGet);
        }
    }
}