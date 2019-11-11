using Sky.Auth.Domain.Models;
using System.Threading.Tasks;

namespace Sky.Auth.Domain.Interfaces
{
    public interface IAuthRepository
    {
        Task<User> CreateUser(User customerWallet);

        Task<User> GetUserByEmail(string email);
    }
}
