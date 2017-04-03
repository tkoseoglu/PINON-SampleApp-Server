using System.Collections.Generic;

namespace PINON.SampleApp.WebApi.Models
{
    public class TokenResponse
    {
        public string access_token { get; set; }
        public CurrentUser current_user { get; set; }
    }

    public class CurrentUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public IList<string> Roles { get; set; }
    }
}