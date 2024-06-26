using GestorPresupuestosAPI.Features.Utility;
using GestorPresupuestosAPI.Infraestructure.Modelos;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

[Route("api/[controller]")]
[ApiController]
public class DepartamentosController : ControllerBase
{
    private readonly DepartamentoService _departamentoService;

    public DepartamentosController(DepartamentoService departamentoService)
    {
        _departamentoService = departamentoService;
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse>> GetAllDepartamentos()
    {
        var response = await _departamentoService.GetAllDepartamentosAsync();
        return Ok(response);
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse>> AddDepartamento([FromBody] Departamentos departamento)
    {
        if (departamento == null)
        {
            return BadRequest(ApiResponse.BadRequest("Invalid departamento data"));
        }

        var response = await _departamentoService.AddDepartamentoAsync(departamento);
        if (!response.Success)
        {
            return BadRequest(response);
        }

        return CreatedAtAction(nameof(GetAllDepartamentos), new { id = departamento.IdDepartamento }, response);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse>> GetDepartamentoById(int id)
    {
        var response = await _departamentoService.GetDepartamentoByIdAsync(id);
        if (!response.Success)
        {
            return NotFound(response);
        }
        return Ok(response);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ApiResponse>> UpdateDepartamento(int id, [FromBody] Departamentos departamento)
    {
        if (departamento == null || id != departamento.IdDepartamento)
        {
            return BadRequest(ApiResponse.BadRequest("Invalid request"));
        }

        var response = await _departamentoService.UpdateDepartamentoAsync(departamento);
        if (!response.Success)
        {
            return BadRequest(response);
        }
        return Ok(response);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<ApiResponse>> DeleteDepartamento(int id)
    {
        var response = await _departamentoService.DeleteDepartamentoAsync(id);
        if (!response.Success)
        {
            return NotFound(response);
        }
        return Ok(response);
    }
}
