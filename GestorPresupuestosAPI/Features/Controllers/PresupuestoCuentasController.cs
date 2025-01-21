using GestorPresupuestosAPI.Features.Utility;
using GestorPresupuestosAPI.Infraestructure.Modelos;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

[Route("api/[controller]")]
[ApiController]
public class PresupuestoCuentasController : ControllerBase
{
    private readonly PresupuestoCuentaService _presupuestoCuentaService;

    public PresupuestoCuentasController(PresupuestoCuentaService presupuestoCuentaService)
    {
        _presupuestoCuentaService = presupuestoCuentaService;
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse>> GetAllPresupuestoCuentas()
    {
        var response = await _presupuestoCuentaService.GetAllPresupuestoCuentasAsync();
        return Ok(response);
    }

    [HttpGet("GetAllCuentasDescriptions")]
    public async Task<ActionResult<List<CuentaDetailDTO>>> GetAllCuentasDescriptions()
    {
        try
        {
            var groupedDetails = await _presupuestoCuentaService.GetAllCuentasDescriptionsAsync();
            if (groupedDetails == null || groupedDetails.Count == 0)
                return NotFound("No account details found.");

            return Ok(groupedDetails);
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Internal server error: " + ex.Message);
        }
    }

    [HttpGet("summary")]
    public async Task<ActionResult<PresupuestoSummaryDTO>> GetPresupuestoSummary()
    {
        try
        {
            var summary = await _presupuestoCuentaService.GetPresupuestoSummaryAsync();
            return Ok(summary);
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Internal server error: " + ex.Message);
        }
    }
    [HttpGet("GetSummaryTipoPresu/{tipopresu}", Name = "GetSummaryTipoPresu")]
    public async Task<ActionResult<ApiResponse>> GetSummaryTipoPresu(int tipopresu)
    {
        var response = await _presupuestoCuentaService.GetSummaryTipoPresu(tipopresu);
        if (!response.Success)
        {
            return NotFound(response);
        }
        return Ok(response);
    }
    [HttpGet("GetSummaryAllPresu", Name = "GetSummaryAllPresu")]
    public async Task<ActionResult<ApiResponse>> GetSummaryAllPresu()
    {
        var response = await _presupuestoCuentaService.GetSummaryAllPresu();
        if (!response.Success)
        {
            return NotFound(response);
        }
        return Ok(response);
    }

    [HttpGet("GetSummaryByIdDepto", Name = "GetSummaryByIdDepto")]
    public async Task<ActionResult<ApiResponse>> GetSummaryByIdDepto(int idDepto)
    {
        var response = await _presupuestoCuentaService.GetSummaryByIdDepto(idDepto);
        if (!response.Success)
        {
            return NotFound(response);
        }
        return Ok(response);
    }

    [HttpGet("GetSummaryById/{idpresu}", Name = "GetSummaryById")]
    public async Task<ActionResult<ApiResponse>> GetSummaryById(int idpresu)
    {
        var response = await _presupuestoCuentaService.GetPresupuestoSummaryById(idpresu);
        if (!response.Success)
        {
            return NotFound(response);
        }
        return Ok(response);
    }


    [HttpPost]
    public async Task<ActionResult<ApiResponse>> AddPresupuestoCuenta([FromBody] PresupuestoCuenta presupuestoCuenta)
    {
        if (presupuestoCuenta == null)
        {
            return BadRequest(ApiResponse.BadRequest("Invalid PresupuestoCuenta data"));
        }

        var response = await _presupuestoCuentaService.AddPresupuestoCuentaAsync(presupuestoCuenta);
        if (!response.Success)
        {
            return BadRequest(response);
        }

        return CreatedAtAction(nameof(GetAllPresupuestoCuentas), new { id = presupuestoCuenta.IdCuentas }, response);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse>> GetPresupuestoCuentaById(int id)
    {
        var response = await _presupuestoCuentaService.GetPresupuestoCuentaByIdAsync(id);
        if (!response.Success)
        {
            return NotFound(response);
        }
        return Ok(response);
    }

    [HttpPut]
    public async Task<ActionResult<ApiResponse>> UpdatePresupuestoCuenta( [FromBody] PresupuestoCuenta presupuestoCuenta)
    {


        var response = await _presupuestoCuentaService.UpdatePresupuestoCuentaAsync(presupuestoCuenta);
        if (!response.Success)
        {
            return BadRequest(response);
        }
        return Ok(response);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<ApiResponse>> DeletePresupuestoCuenta(int id)
    {
        var response = await _presupuestoCuentaService.DeletePresupuestoCuentaAsync(id);
        if (!response.Success)
        {
            return NotFound(response);
        }
        return Ok(response);
    }

    [HttpGet("DesactivarEjeParcial/{id}", Name = "DesactivarEjeParcial")]
    public async Task<ActionResult<ApiResponse>> DesactivarEjeParcial(int id)
    {
        var response = await _presupuestoCuentaService.DesactivarEjeParcial(id);
        if (!response.Success)
        {
            return NotFound(response);
        }
        return Ok(response);
    }
    [HttpGet("desactivarItemPresupuesto/{id}", Name = "desactivarItemPresupuesto")]
    public async Task<ActionResult<ApiResponse>> desactivarItemPresupuesto(int id)
    {
        var response = await _presupuestoCuentaService.desactivarItemPresupuesto(id);
        if (!response.Success)
        {
            return NotFound(response);
        }
        return Ok(response);
    }

    [HttpGet("DesactivarEjecucion/{id}", Name = "DesactivarEjecucion")]
    public async Task<ActionResult<ApiResponse>> DesactivarEjecucion(int id)
    {
        var response = await _presupuestoCuentaService.DesactivarEjecucion(id);
        if (!response.Success)
        {
            return NotFound(response);
        }
        return Ok(response);
    }

    [HttpGet("GetAllCuentasById/{idpresu}", Name = "GetAllCuentasById")]
    public async Task<ActionResult<ApiResponse>> GetAllCuentasId(int idpresu)
    {
        var response = await _presupuestoCuentaService.GetAllCuentasById(idpresu);
        if (!response.Success)
        {
            return NotFound(response);
        }
        return Ok(response);
    }

    [HttpPost("AddItemEjecutados", Name = "AddItemEjecutados")]
    public async Task<ActionResult<ApiResponse>> AddItemsEjecutados([FromBody] ItemsEjecutados item)
    {
        var response = await _presupuestoCuentaService.AddItemsEjecutadosAsync(item);
        if (!response.Success)
        {
            return BadRequest(response);
        }
        return Ok(response);
    }
    [HttpPost("AddItemEjecutadosAdmin", Name = "AddItemEjecutadosAdmin")]
    public async Task<ActionResult<ApiResponse>> AddItemsEjecutadosAdmin([FromBody] ItemsConNoti elementos)
    {
        var response = await _presupuestoCuentaService.AddItemsEjecutadosAdminAsync(elementos);
        if (!response.Success)
        {
            return BadRequest(response);
        }
        return Ok(response);
    }
    [HttpPost("AddItemEjecucionParcialAdmin", Name = "AddItemEjecucionParcialAdmin")]
    public async Task<ActionResult<ApiResponse>> AddItemEjecucionParcialAdmin([FromBody] ItemsParcialesConNoti elementos)
    {
        var response = await _presupuestoCuentaService.AddItemsEjecucionParcialAdminAsync(elementos);
        if (!response.Success)
        {
            return BadRequest(response);
        }
        return Ok(response);
    }

    [HttpPost("AddItemEjecutadosParcial", Name = "AddItemEjecutadosParcial")]
    public async Task<ActionResult<ApiResponse>> AddItemEjecutadosParcial([FromBody] ItemsEjecucionParcial item)
    {
        var response = await _presupuestoCuentaService.AddItemsEjecucionParcialAsync(item);
        if (!response.Success)
        {
            return BadRequest(response);
        }
        return Ok(response);
    }
    [HttpPut("UpdatePresupuestoCuentaAdmin", Name = "UpdatePresupuestoCuentaAdmin")]
    public async Task<IActionResult> UpdatePresupuestoCuenta([FromBody] PresupuestoCuentaUpdateDTO dto)
    {
        if (!ModelState.IsValid)
            return BadRequest("Invalid data.");

        var response = await _presupuestoCuentaService.UpdatePresupuestoCuentaAsync(dto);
        if (!response.Success)
            return BadRequest(response);

        return Ok(response);
    }
    [HttpGet("GetCuentaResumen/{idcuenta}", Name = "GetCuentaResumen")]
    public async Task<ActionResult<ApiResponse>> GetCuentaResumen(int idcuenta)
    {
        var response = await _presupuestoCuentaService.FindItemCuentaResumen(idcuenta);
        if (!response.Success)
        {
            return NotFound(response);
        }
        return Ok(response);
    }
}
