using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace PINON.SampleApp.Identity.Models
{
    public class UserAccount : IdentityUser
    {
        [StringLength(75)]
        [Required]
        public string FirstName { get; set; }

        [StringLength(75)]
        [Required]
        public string LastName { get; set; }
       
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<UserAccount> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class UserAccountSimple
    {       
        public string FirstName { get; set; }       
        public string LastName { get; set; }
        public string Email { get; set; }
        public int? HospitalId { get; set; }
    }
}