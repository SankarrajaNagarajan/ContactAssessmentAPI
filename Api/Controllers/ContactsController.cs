using ContactApi.Domain.Common;
using ContactApi.Domain.DTOs;
using ContactApi.Domain.Models;
using Data.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ContactApi.Api.Controllers;

//[ApiController]
//[Route("api/[controller]")]
////[Authorize]
//public class ContactsController : ControllerBase
//{
//    private readonly IContactRepository _contactRepository;

//    public ContactsController(IContactRepository contactRepository)
//    {
//        _contactRepository = contactRepository;
//    }

//    [HttpGet]
//    public async Task<ActionResult<PagedResult<Contact>>> Get(
//        [FromQuery] int page = 1,
//        [FromQuery] int pageSize = 10,
//        [FromQuery] string? sortBy = null,
//        [FromQuery] string? sortOrder = "desc")
//    {
//        var result = await _contactRepository.GetPagedAsync(page, pageSize, sortBy, sortOrder);
//        return Ok(result);
//    }

//    [HttpGet("{id}")]
//    public async Task<ActionResult<Contact>> Get(int id)
//    {
//        var contact = await _contactRepository.GetByIdAsync(id);
//        return contact != null ? Ok(contact) : NotFound();
//    }

//    [HttpPost]
//    public async Task<ActionResult<Contact>> Post([FromBody] ContactDto dto)
//    {
//        var contact = new Contact
//        {
//            FirstName = dto.FirstName,
//            LastName = dto.LastName,
//            Email = dto.Email,
//            PhoneNumber = dto.PhoneNumber,
//            Address = dto.Address,
//            City = dto.City,
//            State = dto.State,
//            Country = dto.Country,
//            PostalCode = dto.PostalCode
//        };

//        var added = await _contactRepository.AddAsync(contact);
//        return CreatedAtAction(nameof(Get), new { id = added.Id }, added);
//    }

//    [HttpPut("{id}")]
//    public async Task<IActionResult> Put(int id, [FromBody] ContactDto dto)
//    {
//        var contact = await _contactRepository.GetByIdAsync(id);
//        if (contact == null) return NotFound();

//        contact.FirstName = dto.FirstName;
//        contact.LastName = dto.LastName;
//        contact.Email = dto.Email;
//        contact.PhoneNumber = dto.PhoneNumber;
//        contact.Address = dto.Address;
//        contact.City = dto.City;
//        contact.State = dto.State;
//        contact.Country = dto.Country;
//        contact.PostalCode = dto.PostalCode;

//        await _contactRepository.UpdateAsync(contact);
//        return NoContent();
//    }

//    [HttpDelete("{id}")]
//    public async Task<IActionResult> Delete(int id)
//    {
//        await _contactRepository.DeleteAsync(id);
//        return NoContent();
//    }
//}

[ApiController]
[Route("api/[controller]")]
[Authorize] 
public class ContactsController : ControllerBase
{
    private readonly IContactRepository _contactRepository;
    private readonly ILogger<ContactsController> _logger;

    public ContactsController(IContactRepository contactRepository, ILogger<ContactsController> logger)
    {
        _contactRepository = contactRepository;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<PagedResult<Contact>>> Get(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? sortBy = null,
        [FromQuery] string? sortOrder = "desc")
    {
        try
        {
            var result = await _contactRepository.GetPagedAsync(page, pageSize, sortBy, sortOrder);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while fetching paged contacts.");
            return StatusCode(500, "Internal server error while retrieving data.");
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Contact>> Get(int id)
    {
        try
        {
            var contact = await _contactRepository.GetByIdAsync(id);
            if (contact == null)
            {
                _logger.LogWarning("Contact with ID {Id} not found.", id);
                return NotFound();
            }
            return Ok(contact);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while fetching contact {Id}.", id);
            return StatusCode(500, "Internal server error.");
        }
    }

    [HttpPost]
    public async Task<ActionResult<Contact>> Post([FromBody] ContactDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        try
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
                PostalCode = dto.PostalCode,
            };

            var added = await _contactRepository.AddAsync(contact);
            _logger.LogInformation("Successfully created contact with ID {Id}.", added.Id);
            return CreatedAtAction(nameof(Get), new { id = added.Id }, added);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while creating a new contact.");
            return StatusCode(500, "Error saving the contact.");
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, [FromBody] ContactDto dto)
    {
        try
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
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while updating contact {Id}.", id);
            return StatusCode(500, "Error updating the contact.");
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var contact = await _contactRepository.GetByIdAsync(id);
            if (contact == null) return NotFound();

            await _contactRepository.DeleteAsync(id);
            _logger.LogInformation("Deleted contact with ID {Id}.", id);
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while deleting contact {Id}.", id);
            return StatusCode(500, "Error deleting the contact.");
        }
    }
}
