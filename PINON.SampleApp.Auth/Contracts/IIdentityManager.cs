using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using PINON.SampleApp.Common;
using PINON.SampleApp.Identity.Models;

namespace PINON.SampleApp.Identity.Contracts
{
    public interface IIdentityManager
    {
        Task<TransactionResult> PasswordSignInAsync(string email, string password);
        bool SignOut();
        Task<UserAccount> FindByEmailAsync(string email);
        IQueryable<UserAccount> GetUserAccounts();
        UserAccount FindByEmail(string email);
        Task<IList<string>> GetRolesAsync(string userId);
        Task<TransactionResult> RegisterAsync(RegisterViewModel registerModel, string role, string hospitalName);
        Task<IdentityResult> DeleteAsync(string userId);
        Task<IdentityResult> ConfirmEmailAsync(string userId, string token);
    }
}