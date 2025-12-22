using Microsoft.AspNetCore.Mvc;
using Person.Application.Contracts.Requests;
using Person.Application.Services.Interfaces;

namespace Person.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PersonController : ControllerBase
{
    private readonly IPersonService _personService;

    public PersonController(IPersonService personService)
    {
        _personService = personService;
    }

    /// <summary>
    /// Create a person
    /// </summary>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Create([FromBody] CreatePersonRequest request, CancellationToken ct)
    {
        var result = await _personService.CreateAsync(request, ct);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    /// <summary>
    /// Update a person
    /// </summary>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdatePersonRequest request, CancellationToken ct)
    {
        var result = await _personService.UpdateAsync(id, request, ct);
        return Ok(result);
    }

    /// <summary>
    /// Get a person by ID
    /// </summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
    {
        var result = await _personService.GetByIdAsync(id, ct);
        return Ok(result);
    }

    /// <summary>
    /// Get all persons
    /// </summary>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(CancellationToken ct)
    {
        var result = await _personService.GetAllAsync(ct);
        return Ok(result);
    }

    /// <summary>
    /// Delete a person
    /// </summary>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        var result = await _personService.DeleteAsync(id, ct);
        return Ok(result);
    }

    /// <summary>
    /// Add work experience to a person
    /// </summary>
    [HttpPost("{id:guid}/work-experience")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AddWorkExperience(Guid id, [FromBody] AddWorkExperienceRequest request,
        CancellationToken ct)
    {
        var result = await _personService.AddWorkExperienceAsync(id, request, ct);
        return Ok(result);
    }

    /// <summary>
    /// Update person's work experience
    /// </summary>
    [HttpPut("{id:guid}/work-experience/{workExperienceId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateWorkExperience(Guid id, Guid workExperienceId,
        [FromBody] UpdateWorkExperienceRequest request, CancellationToken ct)
    {
        var result = await _personService.UpdateWorkExperienceAsync(id, workExperienceId, request, ct);
        return Ok(result);
    }

    /// <summary>
    /// Delete person's work experience
    /// </summary>
    [HttpDelete("{id:guid}/work-experience/{workExperienceId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteWorkExperience(Guid id, Guid workExperienceId, CancellationToken ct)
    {
        var result = await _personService.DeleteWorkExperienceAsync(id, workExperienceId, ct);
        return Ok(result);
    }
}