using ContactApi.Data.Repositories;
using ContactApi.Domain.Common;
using ContactApi.Domain.DTOs;
using ContactApi.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ContactApi.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
//[Authorize]
public class ContactsController : ControllerBase
{
    private readonly IContactRepository _contactRepository;

    public ContactsController(IContactRepository contactRepository)
    {
        _contactRepository = contactRepository;
    }

    [HttpGet]
    public async Task<ActionResult<PagedResult<Contact>>> Get(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? sortBy = null,
        [FromQuery] string? sortOrder = "desc")
    {
        var result = await _contactRepository.GetPagedAsync(page, pageSize, sortBy, sortOrder);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Contact>> Get(int id)
    {
        var contact = await _contactRepository.GetByIdAsync(id);
        return contact != null ? Ok(contact) : NotFound();
    }

    [HttpPost]
    public async Task<ActionResult<Contact>> Post([FromBody] ContactDto dto)
    {
        var contact = new Contact
        {
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Email = dto.Email,
            PhoneNumber = dto.PhoneNumber,
            Address = dto.Address,
            City = dto.City,
            State = dto.State,
            Country = dto.Country,
            PostalCode = dto.PostalCode
        };

        var added = await _contactRepository.AddAsync(contact);
        return CreatedAtAction(nameof(Get), new { id = added.Id }, added);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, [FromBody] ContactDto dto)
    {
        var contact = await _contactRepository.GetByIdAsync(id);
        if (contact == null) return NotFound();

        contact.FirstName = dto.FirstName;
        contact.LastName = dto.LastName;
        contact.Email = dto.Email;
        contact.PhoneNumber = dto.PhoneNumber;
        contact.Address = dto.Address;
        contact.City = dto.City;
        contact.State = dto.State;
        contact.Country = dto.Country;
        contact.PostalCode = dto.PostalCode;

        await _contactRepository.UpdateAsync(contact);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _contactRepository.DeleteAsync(id);
        return NoContent();
    }
}
