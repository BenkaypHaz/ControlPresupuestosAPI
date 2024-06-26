using GestorPresupuestosAPI.Features.Utility;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

[Route("api/[controller]")]
[ApiController]
public class CuentasController : ControllerBase
{
    private readonly CuentaService _cuentaService;

    public CuentasController(CuentaService cuentaService)
    {
        _cuentaService = cuentaService;
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse>> GetAllCuentas()
    {
        var response = await _cuentaService.GetAllCuentasAsync();
        return Ok(response);
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse>> AddCuenta([FromBody] Cuenta cuenta)
    {
        if (cuenta == null)
        {
            return BadRequest(ApiResponse.BadRequest("Invalid cuenta data"));
        }

        var response = await _cuentaService.AddCuentaAsync(cuenta);
        if (!response.Success)
        {
            return BadRequest(response);
        }

        return CreatedAtAction(nameof(GetAllCuentas), new { id = cuenta.IdCuenta }, response);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse>> GetCuentaById(int id)
    {
        var response = await _cuentaService.GetCuentaByIdAsync(id);
        if (!response.Success)
        {
            return NotFound(response);
        }
        return Ok(response);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ApiResponse>> UpdateCuenta(int id, [FromBody] Cuenta cuenta)
    {
        if (cuenta == null || id != cuenta.IdCuenta)
        {
            return BadRequest(ApiResponse.BadRequest("Invalid request"));
        }

        var response = await _cuentaService.UpdateCuentaAsync(cuenta);
        if (!response.Success)
        {
            return BadRequest(response);
        }
        return Ok(response);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<ApiResponse>> DeleteCuenta(int id)
    {
        var response = await _cuentaService.DeleteCuentaAsync(id);
        if (!response.Success)
        {
            return NotFound(response);
        }
        return Ok(response);
    }
}
