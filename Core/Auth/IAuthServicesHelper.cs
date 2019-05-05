using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;

namespace HomeCTRL.Backend.Core.Auth
{
    public interface IAuthServicesHelper
    {
        /// <summary>
        /// Configures token-based authentication for an
        /// ASP.NET MVC/WebApi application
        /// </summary>
        void ConfigureAuthenticationServices();

        /// <summary>
        /// Generates a new JWT token
        /// </summary>
        /// <param name="identityClaims">Array with identity claims, such as ID and name</param>
        /// <param name="roleClaims">Array with role claims (can be ommited)</param>
        /// <param name="expirationDate">The date for which this token expires, default to 1 day</param>
        /// <param name="signingKey">The key used for signing. Will use the one defined in the constructor if empty/null</param>
        /// <returns>String as the JWT token</returns>
        string GenerateToken(Claim[] identityClaims, Claim[] roleClaims = null, DateTime? expirationDate = null, string signingKey = null);

        /// <summary>
        /// Validates a specific token
        /// </summary>
        /// <param name="token">The token</param>
        /// <param name="signingKey">The key used to sign tokens (if null, will use the key provided in constructor)</param>
        /// <returns>true when valid; false otherwise</returns>
        bool ValidateToken(string token, string signingKey = null);

        /// <summary>
        /// Reads a JWT token
        /// </summary>
        /// <param name="token">The token to read</param>
        /// <param name="signingKey">The key used to sign the token (if null will use the key from constructor)</param>
        /// <exception cref="ArgumentException">If the key is invalid</exception>
        /// <returns>JwtSecurityToken</returns>
        JwtSecurityToken ReadToken(string token, string signingKey = null);
    }
}