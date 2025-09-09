using Microsoft.AspNetCore.Mvc;
using PeopleCrud.Models;
using PeopleCrud.Repositories;

namespace PeopleCrud.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PeopleController : ControllerBase
{
    private readonly IPersonRepository _repository;

    public PeopleController(IPersonRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Person>>> GetAll()
    {
        var people = await _repository.GetAllAsync();
        return Ok(people);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Person>> GetById(int id)
    {
        var person = await _repository.GetByIdAsync(id);
        if (person == null) return NotFound();
        return Ok(person);
    }

    [HttpPost]
    public async Task<ActionResult<Person>> Create(Person person)
    {
        var id = await _repository.CreateAsync(person);
        person.Id = id;
        return CreatedAtAction(nameof(GetById), new { id }, person);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, Person person)
    {
        if (id != person.Id) return BadRequest();
        
        var updated = await _repository.UpdateAsync(person);
        if (!updated) return NotFound();
        
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _repository.DeleteAsync(id);
        if (!deleted) return NotFound();
        
        return NoContent();
    }
}