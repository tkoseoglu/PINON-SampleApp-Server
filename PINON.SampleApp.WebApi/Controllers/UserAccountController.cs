using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using PINON.SampleApp.Common;
using PINON.SampleApp.Identity.Contracts;
using PINON.SampleApp.Tokens;
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
        public ActionResult SignOut()
        {
            var identityLoginResult = _identityManager.SignOut();
            return Json(identityLoginResult, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> SignIn(LoginViewModel model)
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
            var user = await _identityManager.FindByEmailAsync(model.Email);
            var userRole = await _identityManager.GetRolesAsync(user.Id);
            var jwtToken = JwtManager.GenerateToken(model.Email, userRole.FirstOrDefault());
           
            var response = new
            {
                access_token = jwtToken,
                current_user = new
                {
                    user.FirstName,
                    user.LastName,
                    user.Email,
                    Roles = userRole
                }
            };
            return Json(response, JsonRequestBehavior.AllowGet);
        }
    }
}