using System.Threading.Tasks;
using HomeCTRL.Backend.Core.Database;
using HomeCTRL.Backend.Core.Database.Repository;

namespace HomeCTRL.Backend.Features.Users.Repository
{
    public interface IUserRepository : IBaseEntityRepository<UserEntity>
    {
        /// <summary>
        /// Gets a user by the username and password of the user
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns>UserEntity when credentials valid; null otherwise</returns>
        Task<UserEntity> GetByUsernameAndPassword(string email, string password);
    }
}