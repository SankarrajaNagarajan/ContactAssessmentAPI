using System.Threading.Tasks;
using ContactApi.Domain.Common;
using ContactApi.Domain.Models;

namespace ContactApi.Data.Repositories;

public interface IContactRepository
{
    Task<PagedResult<Contact>> GetPagedAsync(int page = 1, int pageSize = 10, string? sortBy = null, string? sortOrder = "desc");
    Task<Contact> GetByIdAsync(int id);
    Task<Contact> AddAsync(Contact contact);
    Task UpdateAsync(Contact contact);
    Task DeleteAsync(int id);
}
