using System;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;

namespace HomeCTRL.Backend.Core.Auth
{
    public class AuthServicesHelper : IAuthServicesHelper
    {
        private IServiceCollection Services { get; set; }
        private string TokenSecret { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="services">Reference to the services</param>
        /// <param name="tokenSecret">The secret token key for signing the JWT tokens</param>
        public AuthServicesHelper(IServiceCollection services, string tokenSecret)
        {
            this.Services = services;
            this.TokenSecret = tokenSecret;
        }

        /// <summary>
        /// Configures token-based authentication for an
        /// ASP.NET MVC/WebApi application
        /// </summary>
        public void ConfigureAuthenticationServices()
        {
            var key = Encoding.ASCII.GetBytes(this.TokenSecret);

            this.Services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

            this.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            this.Services.AddTransient<IPrincipal>(
                provider => provider.GetService<IHttpContextAccessor>().HttpContext.User);
        }

        /// <summary>
        /// Generates a new JWT token
        /// </summary>
        /// <param name="identityClaims">Array with identity claims, such as ID and name</param>
        /// <param name="roleClaims">Array with role claims (can be ommited)</param>
        /// <param name="expirationDate">The date for which this token expires, default to 1 day</param>
        /// <param name="signingKey">The key used for signing. Will use the one defined in the constructor if empty/null</param>
        /// <returns>String as the JWT token</returns>
        public string GenerateToken(
            Claim[] identityClaims, 
            Claim[] roleClaims = null, 
            DateTime? expirationDate = null,
            string signingKey = null)
        {
            IdentityModelEventSource.ShowPII = false;
            
            var tokenHandler = new JwtSecurityTokenHandler();
            byte[] key;

            if (signingKey == null)
                key = Encoding.ASCII.GetBytes(this.TokenSecret);
            else
                key = Encoding.ASCII.GetBytes(signingKey);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(identityClaims),
                Expires = expirationDate ?? DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            if (roleClaims != null)
            {
                foreach (Claim roleClaim in roleClaims)
                {
                    tokenDescriptor.Subject.AddClaim(roleClaim);
                }
            }

            return tokenHandler.WriteToken(tokenHandler.CreateToken(tokenDescriptor));
        }

        /// <summary>
        /// Validates a specific token
        /// </summary>
        /// <param name="token">The token</param>
        /// <param name="signingKey">The key used to sign tokens (if null, will use the key provided in constructor)</param>
        /// <returns>true when valid; false otherwise</returns>
        public bool ValidateToken(string token, string signingKey = null)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            string key;

            if (signingKey == null)
                key = this.TokenSecret;
            else
                key = signingKey;

            try 
            {
                SecurityToken validatedToken;
                tokenHandler.ValidateToken(token, this.GetValidationParameters(key), out validatedToken);

                return true;
            }
            catch (Exception e)
            {
                // Invalid token
            }

            return false;
        }

        /// <summary>
        /// Reads a JWT token
        /// </summary>
        /// <param name="token">The token to read</param>
        /// <param name="signingKey">The key used to sign the token (if null will use the key from constructor)</param>
        /// <exception cref="ArgumentException">If the key is invalid</exception>
        /// <returns>JwtSecurityToken</returns>
        public JwtSecurityToken ReadToken(string token, string signingKey = null)
        {
            if (!this.ValidateToken(token, signingKey))
                throw new ArgumentException("Invalid token key");

            return new JwtSecurityTokenHandler().ReadJwtToken(token);
        }

        /// <summary>
        /// Gets the TokenValidationParameters for validating a JWT token
        /// </summary>
        /// <param name="signingKey">The key used to signed the token</param>
        /// <returns></returns>
        private TokenValidationParameters GetValidationParameters(string signingKey)
        {
            return new TokenValidationParameters()
            {
                ValidateLifetime = false,
                ValidateAudience = false, 
                ValidateIssuer = false,  
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signingKey))
            };
        }
    }
}