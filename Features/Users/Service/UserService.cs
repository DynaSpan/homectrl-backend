using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using HomeCTRL.Backend.Api.DTO;
using HomeCTRL.Backend.Core.Auth;
using HomeCTRL.Backend.Core.Exceptions;
using HomeCTRL.Backend.Features.Users.Repository;

namespace HomeCTRL.Backend.Features.Users.Service
{
    public class UserService : IUserService
    {
        private readonly IUserRepository userRepository;
        private readonly IAuthServicesHelper authServicesHelper;

        public UserService(IUserRepository userRepository, IAuthServicesHelper authServicesHelper)
        {
            this.userRepository = userRepository;
            this.authServicesHelper = authServicesHelper;
        }

        public async Task<TokenDTO> AuthenticateUser(LoginInfoDTO loginInfo) 
        {
            var userEntity = await this.userRepository.GetByUsernameAndPassword(loginInfo.Username, loginInfo.Password);
            var user = Mapper.Map<User>(userEntity);

            if (user == null) 
                throw new InputException("INVALID_CREDENTIALS");

            var identityClaims = new Claim[] {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString("D")),
                new Claim(ClaimTypes.Name, user.Name)
            };

            // For future implementation
            // var roleClaims = new Claim[user.Permissions.Length];
            
            // var i = 0;
            // foreach (string p in user.Permissions) 
            // {
            //     roleClaims.SetValue(new Claim(ClaimTypes.Role, p), i);
            //     ++i;
            // }

            var tokenDTO = new TokenDTO();
            tokenDTO.Token = this.authServicesHelper.GenerateToken(identityClaims, null, DateTime.UtcNow.AddDays(1));
            
            return tokenDTO;
        }

        public async Task<IEnumerable<User>> GetAll()
        {
            return Mapper.Map<IEnumerable<User>>(await this.userRepository.GetAll());
        }

        public async Task<User> Get(Guid id)
        {
            return Mapper.Map<User>(await this.userRepository.Get(id));
        }
    }
}