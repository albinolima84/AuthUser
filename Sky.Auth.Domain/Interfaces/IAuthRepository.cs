using Sky.Auth.Domain.Models;
using System.Threading.Tasks;

namespace Sky.Auth.Domain.Interfaces
{
    public interface IAuthRepository
    {
        Task<User> CreateUser(User user);

        Task<User> GetUserByEmail(string email);

        Task<User> Authenticate(string email, string password);

        Task<User> UpdateLastLogin(User user);

        Task<User> GetUserById(string objectId);
    }
}
