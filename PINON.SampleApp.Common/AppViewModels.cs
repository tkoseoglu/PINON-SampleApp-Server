using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace PINON.SampleApp.Common
{
    public class LoginViewModel
    {
        [Required]        
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]        
        public string Password { get; set; }
        
        public int? HospitalId { get; set; }

        public bool RememberMe { get; set; }
    }

    public class RegisterViewModel
    {
        [Required]        
        public string FirstName { get; set; }

        [Required]        
        public string LastName { get; set; }

        [Required]
        [EmailAddress]        
        public string Email { get; set; }

        [Required]        
        public int HospitalId { get; set; }

        [Required]       
        [DataType(DataType.Password)]        
        public string Password { get; set; }        
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