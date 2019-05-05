using System.Threading.Tasks;
using HomeCTRL.Backend.Api.DTO;

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
    }
}