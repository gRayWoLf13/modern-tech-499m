using System.Collections.Generic;
using System.Threading.Tasks;
using modern_tech_499m.Repositories.Core.Domain;

namespace modern_tech_499m.Repositories.Core.Repositories
{
    public interface IUserRepository : IRepository<User>
    {
        IEnumerable<User> GetUserFromGame(int id);

        Task<(LoginResult loginResult, User loggedUser)> LoginUser(string username, byte[] passwordHash);

        Task<(RegisterResult registerResult, User registeredUser)> RegisterUser(User user);
    }
}
