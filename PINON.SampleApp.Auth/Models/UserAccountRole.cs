using Microsoft.AspNet.Identity.EntityFramework;

namespace PINON.SampleApp.Auth.Models
{
    public class UserAccountRole : IdentityRole
    {
        public bool IsDeletable { get; set; }
    }
}
