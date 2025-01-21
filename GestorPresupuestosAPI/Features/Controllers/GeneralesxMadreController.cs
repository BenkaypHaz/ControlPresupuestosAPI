using GestorPresupuestosAPI.Features.Services;
using GestorPresupuestosAPI.Features.Utility;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class GeneralesxMadreController : ControllerBase
{
    private readonly GeneralesxMadreService _service;

    public GeneralesxMadreController(GeneralesxMadreService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var response = await _service.GetAllAsync();
        return StatusCode(response.Success ? StatusCodes.Status200OK : StatusCodes.Status400BadRequest, response);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var response = await _service.GetByIdAsync(id);
        if (!response.Success)
        {
            return StatusCode(StatusCodes.Status404NotFound, response);
        }
        return Ok(response);
    }

    [HttpPost]
    public async Task<IActionResult> Add([FromBody] GeneralesxMadre dto)
    {
        var response = await _service.AddAsync(dto);
        if (response.Success)
        {
            var addedEntity = (GeneralesxMadre)response.Data;
            return CreatedAtAction(nameof(GetById), new { id = addedEntity.Id }, response);
        }
        return StatusCode(StatusCodes.Status400BadRequest, response);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] GeneralesxMadre dto)
    {
        if (id != dto.Id)
        {
            return BadRequest(ApiResponse.BadRequest("ID mismatch."));
        }

        var response = await _service.UpdateAsync(dto);
        return StatusCode(response.Success ? StatusCodes.Status200OK : StatusCodes.Status400BadRequest, response);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var response = await _service.DeleteAsync(id);
        return StatusCode(response.Success ? StatusCodes.Status204NoContent : StatusCodes.Status400BadRequest, response);
    }
}
