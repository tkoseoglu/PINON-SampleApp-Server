using System.Collections.Generic;
using System.Threading.Tasks;
using PINON.SampleApp.Common;
using PINON.SampleApp.Identity.Models;

namespace PINON.SampleApp.Identity.Contracts
{
    public interface IIdentityManager
    {
        Task<TransactionResult> PasswordSignInAsync(string email, string password);
        bool SignOut();
        Task<UserAccount> FindByEmailAsync(string email);
        Task<IList<string>> GetRolesAsync(string userId);
    }
}