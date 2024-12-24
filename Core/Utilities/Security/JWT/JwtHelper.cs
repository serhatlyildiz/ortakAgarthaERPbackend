using Core.Entities.Concrete;
using Core.Extensions;
using Core.Utilities.Security.Encryption;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Core.Utilities.Security.JWT
{
    public class JwtHelper : ITokenHelper
    {
        public IConfiguration Configuration { get; }
        private TokenOptions _tokenOptions;
        private DateTime _accessTokenExpiration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public JwtHelper(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            Configuration = configuration;
            _tokenOptions = Configuration.GetSection("TokenOptions").Get<TokenOptions>();
            _httpContextAccessor = httpContextAccessor;
        }
        public AccessToken CreateToken(Users user, List<OperationClaim> operationClaims)
        {
            _accessTokenExpiration = DateTime.Now.AddMilliseconds(TimeSpan.FromMinutes(_tokenOptions.AccessTokenExpiration).TotalMilliseconds);
            var securityKey = SecurityKeyHelper.CreateSecurityKey(_tokenOptions.SecurityKey);
            var signingCredentials = SigningCredentialsHelper.CreateSigningCredentials(securityKey);
            var jwt = CreateJwtSecurityToken(_tokenOptions, user, signingCredentials, operationClaims);
            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            var token = jwtSecurityTokenHandler.WriteToken(jwt);

            return new AccessToken
            {
                Token = token,
                Expiration = _accessTokenExpiration
            };

        }
        public JwtSecurityToken CreateJwtSecurityToken(TokenOptions tokenOptions, Users user,
            SigningCredentials signingCredentials, List<OperationClaim> operationClaims)
        {
            var jwt = new JwtSecurityToken(
                issuer: tokenOptions.Issuer,
                audience: tokenOptions.Audience,
                expires: _accessTokenExpiration,
                notBefore: DateTime.Now,
                claims: SetClaims(user, operationClaims),
                signingCredentials: signingCredentials
            );
            return jwt;
        }
        private IEnumerable<Claim> SetClaims(Users user, List<OperationClaim> operationClaims)
        {
            var claims = new List<Claim>();
            claims.AddNameIdentifier(user.Id.ToString());
            claims.AddEmail(user.Email);
            claims.AddName($"{user.FirstName} {user.LastName}");  // Full name
            claims.AddRoles(operationClaims.Select(c => c.Name).ToArray());

            return claims;
        }

        public string? GetUserEmailFromToken()
        {
            Claim? emailClaim = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Email);
            return emailClaim != null || emailClaim?.Value != "" ? emailClaim?.Value : null;
        }
    }
}
