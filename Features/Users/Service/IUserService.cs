using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HomeCTRL.Backend.Core.DTO;

namespace HomeCTRL.Backend.Features.Users.Service
{
    public interface IUserService
    {
        /// <summary>
        /// Authenticates a user and returns a JWT
        /// </summary>
        /// <param name="loginInfo"></param>
        /// <exception cref="InputException">When credentials are invalid</exception>
        /// <returns></returns>
        Task<TokenDTO> AuthenticateUser(LoginInfoDTO loginInfo);

        /// <summary>
        /// Gets all users
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<User>> GetAll();

        /// <summary>
        /// Gets a user by its ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<User> Get(Guid id);
    }
}