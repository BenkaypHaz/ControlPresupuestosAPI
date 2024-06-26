using GestorPresupuestosAPI.Features.Utility;
using GestorPresupuestosAPI.Infraestructure.Modelos;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

[Route("api/[controller]")]
[ApiController]
public class PresupuestosController : ControllerBase
{
    private readonly PresupuestoService _presupuestoService;

    public PresupuestosController(PresupuestoService presupuestoService)
    {
        _presupuestoService = presupuestoService;
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse>> GetAllPresupuestos()
    {
        var response = await _presupuestoService.GetAllPresupuestosAsync();
        return Ok(response);
    }

    [HttpGet("GetTipoPresupuesto")]
    public async Task<ActionResult<ApiResponse>> GetTipoPresupuestos()
    {
        var response = await _presupuestoService.GetTipoPresupuesto();
        return Ok(response);
    }
    [HttpGet("CrearPresupuesto/{nombre}/{tipo}/{usuid}")]
    public async Task<ActionResult<ApiResponse>> GetTipoPresupuestos(string nombre, int tipo, int usuid)
    {
        var response = await _presupuestoService.CrearPresupuestoAsync(nombre, tipo, usuid);
        return Ok(response);
    }


    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse>> GetPresupuestoById(int id)
    {
        var response = await _presupuestoService.GetPresupuestoByIdAsync(id);
        if (!response.Success)
        {
            return NotFound(response);
        }
        return Ok(response);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ApiResponse>> UpdatePresupuesto(int id, [FromBody] Presupuesto presupuesto)
    {
        if (presupuesto == null || id != presupuesto.IdPresu)
        {
            return BadRequest(ApiResponse.BadRequest("Invalid request"));
        }

        var response = await _presupuestoService.UpdatePresupuestoAsync(presupuesto);
        if (!response.Success)
        {
            return BadRequest(response);
        }
        return Ok(response);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<ApiResponse>> DeletePresupuesto(int id)
    {
        var response = await _presupuestoService.DeletePresupuestoAsync(id);
        if (!response.Success)
        {
            return NotFound(response);
        }
        return Ok(response);
    }

    [HttpPost("AddWithCuentas")]
    public async Task<ActionResult<ApiResponse>> AddPresupuestoWithCuentas([FromBody] PresupuestoWithCuentasDTO data)
    {
        if (data.cantidad == null || data.Cuentas == null || !data.Cuentas.Any())
        {
            return BadRequest(ApiResponse.BadRequest("Invalid data provided"));
        }

        var response = await _presupuestoService.AddPresupuestoWithCuentasAsync(data.cantidad, data.Cuentas);
        return StatusCode(response.Success ? StatusCodes.Status201Created : StatusCodes.Status400BadRequest, response);
    }

    [HttpGet("GetPresupuestoById/{idpresu}", Name = "GetPresupuestoById")]
    public async Task<ActionResult<ApiResponse>> GetPresupuestoInfo(int idpresu)
    {
        var response = await _presupuestoService.GetPresupuestoInfo(idpresu);
        if (!response.Success)
        {
            return NotFound(response);
        }
        return Ok(response);
    }


}
