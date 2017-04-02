using System;
using System.Threading.Tasks;

namespace PINON.SampleApp.Web.Tokens
{
    public class JwtIssuerOptions
    {
       
        public string Issuer { get; set; }

       
        public string Subject { get; set; }

       
        public string Audience { get; set; }

       
        public DateTime NotBefore => DateTime.UtcNow;

       
        public DateTime IssuedAt => DateTime.UtcNow;

       
        public TimeSpan ValidFor { get; set; } = TimeSpan.FromMinutes(30);

       
        public DateTime Expiration => IssuedAt.Add(ValidFor);

       
        public Func<Task<string>> JtiGenerator =>
            () => Task.FromResult(Guid.NewGuid().ToString());

      
        public Microsoft.IdentityModel.Tokens.SigningCredentials SigningCredentials { get; set; }
    }
}