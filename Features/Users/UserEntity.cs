using System.ComponentModel.DataAnnotations.Schema;
using HomeCTRL.Backend.Core.Database.Entity;

namespace HomeCTRL.Backend.Features.Users
{
    [Table("`user`")]
    public class UserEntity : Entity
    {
        public string Name { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }
    }
}