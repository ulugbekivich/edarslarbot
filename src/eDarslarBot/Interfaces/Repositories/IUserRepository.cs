using eDarslarBot.Models;

namespace eDarslarBot.Interfaces.Repositories
{
    public interface IUserRepository
    {
        public Task<int> CreateAsync(User user);
    }
}
