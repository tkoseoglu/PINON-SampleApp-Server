using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using PINON.SampleApp.Common;
using PINON.SampleApp.Identity.Contracts;
using PINON.SampleApp.Identity.Models;
using System.Web;
using Microsoft.Owin.Security;

namespace PINON.SampleApp.Identity
{
    public class IdentityManager : IIdentityManager, IDisposable
    {
        private ApplicationUserManager _userManager;
        private ApplicationSignInManager _signInManager;
        private ApplicationSignInManager SignInManager
        {
            get
            {
                return this._signInManager ?? HttpContext.Current.GetOwinContext().Get<ApplicationSignInManager>();
            }
            set
            {
                this._signInManager = value;
            }
        }
        private ApplicationUserManager UserManager
        {
            get
            {
                return this._userManager ?? HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            set
            {
                this._userManager = value;
            }
        }

        private IAuthenticationManager AuthenticationManager
        {
            get { return HttpContext.Current.GetOwinContext().Authentication; }
        }

        public void Dispose()
        {

        }

        public async Task<TransactionResult> PasswordSignInAsync(string email, string password)
        {
            var result = new TransactionResult();
            var signInResult = await SignInManager.PasswordSignInAsync(email, password, true, false);
            if (signInResult == SignInStatus.Success)
                return result;
            result.HasError = true;
            result.Message = $"Sign-In Error {signInResult}";
            return result;
        }

        public bool SignOut()
        {
            this.AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return true;
        }

        public async Task<UserAccount> FindByEmailAsync(string email)
        {
            return await this.UserManager.FindByEmailAsync(email);
        }

        public UserAccount FindByEmail(string email)
        {
            return this.UserManager.FindByEmail(email);
        }

        public async Task<IList<string>> GetRolesAsync(string userId)
        {
            return await this.UserManager.GetRolesAsync(userId);
        }

        public async Task<TransactionResult> RegisterAsync(RegisterViewModel registerModel, string role)
        {
            var result = new TransactionResult();
            var user = new UserAccount
            {
                FirstName = registerModel.FirstName,
                LastName = registerModel.LastName,
                Email = registerModel.Email,
                UserName = registerModel.Email,
                EmailConfirmed = true, //should be false in PROD. For the purpose of this app we are not going to use email confirmations
                PhoneNumberConfirmed = true
            };
            var registerResult = await UserManager.CreateAsync(user, registerModel.Password);
            if (registerResult.Succeeded)
            {
                this.UserManager.AddToRole(user.Id, role);
                return result;
            }                
            result.HasError = true;
            result.Message = $"Register Error {string.Join(", ",registerResult.Errors)}";
            return result;
        }

        public async Task<IdentityResult> DeleteAsync(string userId)
        {
            var user = await this.UserManager.FindByIdAsync(userId);
            user.IsDeleted = true;
            return await this.UserManager.UpdateAsync(user);
        }
       
    }
}