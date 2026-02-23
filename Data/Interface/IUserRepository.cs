using System.Threading.Tasks;
using ContactApi.Domain.Models;

namespace Data.Interface;

public interface IUserRepository
{
    Task<User> GetByUsernameAsync(string username);
    Task<User> GetByIdAsync(int id);
}
