using System.Threading.Tasks;
using HomeCTRL.Backend.Core.Database;
using HomeCTRL.Backend.Core.Database.Repository;

namespace HomeCTRL.Backend.Features.Users.Repository
{
    public class UserRepository : BaseEntityRepository<UserEntity>, IUserRepository
    {
        public UserRepository(IDatabaseFactory dbFactory) : base(dbFactory)
        {
        }

        public async Task<UserEntity> GetByUsernameAndPassword(string username, string password) 
        {
            using (var db = this.DbFactory.GetDatabase())
            {
                var user = await this.Get(u => u.Username == username);

                if (user == null) 
                    return null;

                if (BCrypt.Net.BCrypt.Verify(password, user.Password)) 
                {
                    return user;
                }

                return null;
            }
        }
    }
}