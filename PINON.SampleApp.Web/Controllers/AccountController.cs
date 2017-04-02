using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Newtonsoft.Json;
using PINON.SampleApp.Auth.Models;
using PINON.SampleApp.Common;
using PINON.SampleApp.Data.Contracts.Repos;
using PINON.SampleApp.Data.Models;
using PINON.SampleApp.Web.Models;
using PINON.SampleApp.Web.Tokens;

namespace PINON.SampleApp.Web.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly IPatientRepo _patientRepo;
        private JsonSerializerSettings _serializerSettings;
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public AccountController()
        {
        }

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager,
            IPatientRepo patientRepo)
        {
            UserManager = userManager;
            SignInManager = signInManager;
            _patientRepo = patientRepo;
        }

        public ApplicationSignInManager SignInManager
        {
            get { return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>(); }
            private set { _signInManager = value; }
        }

        public ApplicationUserManager UserManager
        {
            get { return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>(); }
            private set { _userManager = value; }
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            var result = new TransactionResult();

            if (!ModelState.IsValid)
            {
                result.HasError = true;
                result.Message = "Missing login information";
                return Json(result, JsonRequestBehavior.AllowGet);
            }

            var loginResult = await SignInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe,
                false);

            switch (loginResult)
            {
                case SignInStatus.Success:
                    result.HasError = false;
                    result.IsAuthenticated = true;
                    break;
                    //var user = await UserManager.FindByEmailAsync(model.Email);
                    //var isAdmin = await UserManager.IsInRoleAsync(user.Id, "Admin");
                    //var jwtToken = await GenerateJwtToken(user, isAdmin);
                    //return Json(jwtToken, JsonRequestBehavior.AllowGet);
                case SignInStatus.LockedOut:
                    result.HasError = false;
                    break;
                case SignInStatus.RequiresVerification:
                    result.HasError = false;
                    break;
                case SignInStatus.Failure:
                default:
                    result.HasError = true;
                    result.Message = "Invalid login attempt.";
                    break;
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            var result = new TransactionResult();
            if (!ModelState.IsValid)
            {
                result.HasError = true;
                result.Message = "Missing login information";
                return Json(result, JsonRequestBehavior.AllowGet);
            }

            var user = new UserAccount
            {
                UserName = model.Email,
                Email = model.Email
            };
            var registerUserResult = await UserManager.CreateAsync(user, model.Password);
            if (!registerUserResult.Succeeded) return Json(result, JsonRequestBehavior.AllowGet);
            await SignInManager.SignInAsync(user, false, false);

            var newUserAccount = await UserManager.FindByEmailAsync(model.Email);
            var patient = new Patient
            {
                HospitalId = model.HospitalId,
                UserAccountId = newUserAccount.Id
            };
            _patientRepo.Save(patient, newUserAccount.Id);

            var jwtToken = await GenerateJwtToken(newUserAccount, false);
            return Json(jwtToken, JsonRequestBehavior.AllowGet);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }
            }

            base.Dispose(disposing);
        }

        private async Task<string> GenerateJwtToken(UserAccount user, bool isAdmin)
        {
            var jwtFactory = new JwtFactory();

            _serializerSettings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented
            };

            var jwtToken = await jwtFactory.GetJwtTokenAsync(user.Email, isAdmin);

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
            var json = JsonConvert.SerializeObject(response, _serializerSettings);
            return json;
        }

        #region Helpers

        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get { return HttpContext.GetOwinContext().Authentication; }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
                ModelState.AddModelError("", error);
        }

        //private ActionResult RedirectToLocal(string returnUrl)
        //{
        //    if (Url.IsLocalUrl(returnUrl))
        //        return Redirect(returnUrl);
        //    return RedirectToAction("Index", "Home");
        //}

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties {RedirectUri = RedirectUri};
                if (UserId != null)
                    properties.Dictionary[XsrfKey] = UserId;
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }

        #endregion
    }
}