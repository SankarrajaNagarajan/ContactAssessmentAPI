using System.Linq;
using System.Threading.Tasks;
using ContactApi.Data.Data;
using ContactApi.Domain.Common;
using ContactApi.Domain.Models;
using Data.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ContactApi.Data.Repositories;

public class ContactRepository : IContactRepository
{
    private readonly AppDbContext _context;
    private readonly ILogger<ContactRepository> _logger;

    public ContactRepository(AppDbContext context, ILogger<ContactRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<PagedResult<Contact>> GetPagedAsync(int page = 1, int pageSize = 10, string? sortBy = "CreatedAt", string? sortOrder = "desc")
    {
        _logger.LogInformation("Fetching contacts page {Page}, size {PageSize}", page, pageSize);

        var query = _context.Contacts.AsQueryable();

        if (!string.IsNullOrEmpty(sortBy))
        {
            query = sortOrder?.ToLower() == "asc"
                ? query.OrderBy(x => EF.Property<object>(x, sortBy))
                : query.OrderByDescending(x => EF.Property<object>(x, sortBy));
        }
        else
        {
            query = query.OrderByDescending(x => x.CreatedAt);
        }

        var total = await query.CountAsync();
        var items = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new PagedResult<Contact>
        {
            Items = items,
            TotalCount = total,
            Page = page,
            PageSize = pageSize
        };
    }

    public async Task<Contact> GetByIdAsync(int id)
    {
        return await _context.Contacts.FindAsync(id);
    }

    public async Task<Contact> AddAsync(Contact contact)
    {
        _context.Contacts.Add(contact);
        await _context.SaveChangesAsync();
        _logger.LogInformation("Added contact ID: {Id}", contact.Id);
        return contact;
    }

    public async Task UpdateAsync(Contact contact)
    {
        _context.Contacts.Update(contact);
        await _context.SaveChangesAsync();
        _logger.LogInformation("Updated contact ID: {Id}", contact.Id);
    }

    public async Task DeleteAsync(int id)
    {
        var contact = await GetByIdAsync(id);
        if (contact != null)
        {
            _context.Contacts.Remove(contact);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Deleted contact ID: {Id}", id);
        }
    }
}
