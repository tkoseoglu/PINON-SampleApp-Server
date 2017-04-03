using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace PINON.SampleApp.WebApi.Models
{
    public class LoginViewModel
    {
        [Required]
        [Display(Name = "Email")]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }

    public class RegisterViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Hospital")]
        public int HospitalId { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

    public class MenuItem
    {
        public string Route { get; set; }
        public string Name { get; set; }
        public bool IsDivider { get; set; }
        public List<MenuItem> SubTabs { get; set; }

        public List<string> SubTabsRoutes
        {
            get
            {
                var routes = SubTabs?.Select(p => $"/{p.Route}").ToList();
                return routes;
            }
        }
    }
}