﻿using System.Collections.Generic;
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
        UserAccount FindByEmail(string email);
        Task<IList<string>> GetRolesAsync(string userId);
        Task<TransactionResult> RegisterAsync(RegisterViewModel registerModel, string role);
        Task<IdentityResult> DeleteAsync(string userId);
    }
}