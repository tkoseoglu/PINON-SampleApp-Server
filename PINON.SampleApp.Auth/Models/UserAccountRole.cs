using Microsoft.AspNet.Identity.EntityFramework;

namespace PINON.SampleApp.Identity.Models
{
    public class UserAccountRole : IdentityRole
    {
        public bool IsDeletable { get; set; }
    }
}
