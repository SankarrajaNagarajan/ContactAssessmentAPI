using System.Threading.Tasks;
using ContactApi.Domain.Models;

namespace ContactApi.Data.Repositories;

public interface IUserRepository
{
    Task<User?> GetByUsernameAsync(string username);
    Task<User?> GetByIdAsync(int id);
}
