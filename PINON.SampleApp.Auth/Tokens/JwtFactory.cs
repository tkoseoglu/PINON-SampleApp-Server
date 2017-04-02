using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using PINON.SampleApp.Common;

namespace PINON.SampleApp.Auth.Tokens
{
    public class JwtFactory : IJwtFactory
    {
        private readonly JwtIssuerOptions _jwtOptions;

        public JwtFactory()
        {
            var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Constants.JwtSecretKey));
            _jwtOptions = new JwtIssuerOptions
            {
                Issuer = Constants.JwtIssuer,
                Audience = Constants.JwtAudience,
                ValidFor = TimeSpan.FromMinutes(Constants.JwtExpirationInMinutes),
                SigningCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256)
            };

            ThrowIfInvalidOptions(_jwtOptions);
        }

        public async Task<string> GetJwtTokenAsync(string userName, bool isAdmin)
        {
            var identity = new ClaimsIdentity(new GenericIdentity(userName, "Token"), new[]
                {
                    new Claim("Role", isAdmin ? "Admin" : "Patient")
                });

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, userName),
                new Claim(JwtRegisteredClaimNames.Jti, await _jwtOptions.JtiGenerator()),
                new Claim(JwtRegisteredClaimNames.Iat, ToUnixEpochDate(_jwtOptions.IssuedAt).ToString(), ClaimValueTypes.Integer64),
                identity.FindFirst("Role")
            };

            var jwt = new JwtSecurityToken(
                _jwtOptions.Issuer,
                _jwtOptions.Audience,
                claims,
                _jwtOptions.NotBefore,
                _jwtOptions.Expiration,
                _jwtOptions.SigningCredentials);

            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }

        private static void ThrowIfInvalidOptions(JwtIssuerOptions options)
        {
            if (options == null) throw new ArgumentNullException(nameof(options));

            if (options.ValidFor <= TimeSpan.Zero)
                throw new ArgumentException("Must be a non-zero TimeSpan.", nameof(JwtIssuerOptions.ValidFor));

            if (options.SigningCredentials == null)
                throw new ArgumentNullException(nameof(JwtIssuerOptions.SigningCredentials));

            if (options.JtiGenerator == null)
                throw new ArgumentNullException(nameof(JwtIssuerOptions.JtiGenerator));
        }

        private static long ToUnixEpochDate(DateTime date) => (long)
              Math.Round((date.ToUniversalTime() - new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero)).TotalSeconds);
    }
}