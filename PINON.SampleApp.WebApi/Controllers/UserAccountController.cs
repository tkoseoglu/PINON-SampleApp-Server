using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using PINON.SampleApp.Common;
using PINON.SampleApp.Data.Contracts.Repos;
using PINON.SampleApp.Data.Models;
using PINON.SampleApp.Identity.Contracts;
using PINON.SampleApp.Tokens;
using PINON.SampleApp.WebApi.Models;

namespace PINON.SampleApp.WebApi.Controllers
{
    public class UserAccountController : Controller
    {
        private readonly IIdentityManager _identityManager;
        private readonly IPatientRepo _patientRepo;

        public UserAccountController(IIdentityManager identityManager, IPatientRepo patientRepo)
        {
            _identityManager = identityManager;
            _patientRepo = patientRepo;
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

            //sign-in using asp.net identity
            var identityLoginResult = await _identityManager.PasswordSignInAsync(model.Email, model.Password);
            if (identityLoginResult.HasError) return Json(identityLoginResult, JsonRequestBehavior.AllowGet);

            //get user identity
            var user = await _identityManager.FindByEmailAsync(model.Email);
            var userRole = await _identityManager.GetRolesAsync(user.Id);
            //generate jwt token
            var jwtToken = JwtManager.GenerateToken(model.Email, userRole.FirstOrDefault(), user.Id);
           
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

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            var result = new TransactionResult();

            if (!ModelState.IsValid)
            {
                result.HasError = true;
                result.Message = "Missing registration information";
                return Json(result, JsonRequestBehavior.AllowGet);
            }

            //part 1: register using asp.net identity
            var identityRegisterResult = await _identityManager.RegisterAsync(model, "Patient");
            if (identityRegisterResult.HasError) return Json(identityRegisterResult, JsonRequestBehavior.AllowGet);
            var user = await _identityManager.FindByEmailAsync(model.Email);

            //part 2: create patient record
            var patient = new Patient
            {                
                UserAccountId = user.Id,
                HospitalId = model.HospitalId
            };
            var crudResult = _patientRepo.Save(patient, user.Email);            
            return Json(crudResult, JsonRequestBehavior.AllowGet);
        }

        [AllowAnonymous]
        public async Task<string> ConfirmRegistrationFromEmail(string userId, string token)
        {
            token = token.Replace(" ", "+");         
            try
            {
                if ((userId == null) || (token == null))
                {                   
                    return $"An error occurred";
                }
                var confirmEmailResult = await _identityManager.ConfirmEmailAsync(userId, token);
                if (!confirmEmailResult.Succeeded) return string.Join(", ", confirmEmailResult.Errors);

                var siteUser = _identityManager.GetUserAccounts().FirstOrDefault(p => p.Id == userId);
                if (siteUser == null)
                    return "User not found";

                var applicationBaseUrl = Constants.ApplicationUrl;

                var html = $"<div>Hello, {siteUser.FirstName}</div>";
                html += "<div>You have successfully completed your account registration for Pinon Sample App.</div>";
                html += $"<div><a href='{applicationBaseUrl}/#login'>Click here</a> to login.</div><br>";
                html += "<div>-Pinon Sample App Team</div>";
                return html;
            }
            catch (Exception ex)
            {                
                return $"An error occurred {ex.Message}";
            }
        }

        private async Task<TokenResponse> GetTokenResponse(string email)
        {
            //get user identity
            var user = await _identityManager.FindByEmailAsync(email);
            var userRole = await _identityManager.GetRolesAsync(user.Id);
            
            //generate jwt token
            var jwtToken = JwtManager.GenerateToken(email, userRole.FirstOrDefault(), user.Id);

            return new TokenResponse
            {
                access_token = jwtToken,
                current_user = new CurrentUser
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    Roles = userRole
                }
            };
        }
    }
}