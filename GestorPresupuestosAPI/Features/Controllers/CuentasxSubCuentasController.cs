using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using GestorPresupuestosAPI.Features.Utility;
using GestorPresupuestosAPI.Infraestructure.Modelos;
using GestorPresupuestosAPI.Infraestructure.Entities;

[Route("api/[controller]")]
[ApiController]
public class CuentasxSubCuentasController : ControllerBase
{
    private readonly CuentasxSubCuentasService _cuentasxSubCuentasService;

    public CuentasxSubCuentasController(CuentasxSubCuentasService cuentasxSubCuentasService)
    {
        _cuentasxSubCuentasService = cuentasxSubCuentasService;
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse>> GetAll()
    {
        var response = await _cuentasxSubCuentasService.GetAllCuentasxSubCuentasAsync();
        if (!response.Success)
        {
            return BadRequest(response);
        }
        return Ok(response);
    }
    [HttpGet("GetCuentasMadre", Name = "GetCuentasMadre")]
    public async Task<ActionResult<ApiResponse>> GetCuentasMadre()
    {
        var response = await _cuentasxSubCuentasService.GetCuentasMadre();
        if (!response.Success)
        {
            return NotFound(response);

        }
        return Ok(response);

    }

    [HttpGet("GetCuentasTitulo", Name = "GetCuentasTitulo")]
    public async Task<ActionResult<ApiResponse>> GetCuentasTitulo()
    {
        var response = await _cuentasxSubCuentasService.GetCuentasTitulo();
        if (!response.Success)
        {
            return NotFound(response);

        }
        return Ok(response);

    }

    [HttpPost("AddCuentaMadre")]
    public async Task<ActionResult<ApiResponse>> AddCuentaMadre([FromBody] Cuentas_Madre data)
    {

        var response = await _cuentasxSubCuentasService.AddCuentaMadre(data);
        return StatusCode(response.Success ? StatusCodes.Status201Created : StatusCodes.Status400BadRequest, response);
    }


    [HttpPost("AddCuentaTitulo")]
    public async Task<ActionResult<ApiResponse>> AddCuentaTitulo([FromBody] Cuentas_Generales data)
    {

        var response = await _cuentasxSubCuentasService.AddCuentaTitulo(data);
        return StatusCode(response.Success ? StatusCodes.Status201Created : StatusCodes.Status400BadRequest, response);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse>> GetById(int id)
    {
        var response = await _cuentasxSubCuentasService.GetCuentasxSubCuentasByIdAsync(id);
        if (!response.Success)
        {
            return NotFound(response);
        }
        return Ok(response);
    }
    [HttpGet("GetSubCuentasByIdCuenta/{id}", Name = "GetSubCuentasByIdCuenta")]
    public async Task<ActionResult<ApiResponse>> GetSubCuentasByIdCuenta(int id)
    {
        var response = await _cuentasxSubCuentasService.GetSubCuentasByIdCuenta(id);
        if (!response.Success)
        {
            return NotFound(response);
        }
        return Ok(response);
    }
    [HttpGet("GetCuentasMadreById/{id}", Name = "GetCuentasMadreById")]
    public async Task<ActionResult<ApiResponse>> GetCuentasMadreById(int id)
    {
        var response = await _cuentasxSubCuentasService.GetCuentasMadreById(id);
        if (!response.Success)
        {
            return NotFound(response);
        }
        return Ok(response);
    }
    [HttpPost]
    public async Task<ActionResult<ApiResponse>> Add([FromBody] CuentasxSubCuentas cuentasxSubCuentas)
    {
        var response = await _cuentasxSubCuentasService.AddCuentasxSubCuentasAsync(cuentasxSubCuentas);
        if (!response.Success)
        {
            return BadRequest(response);
        }
        return CreatedAtAction(nameof(GetById), new { id = ((CuentasxSubCuentas)response.Data).Id }, response);
    }

    [HttpPut("UpdateCuentaMadre", Name = "UpdateCuentaMadre")]
    public async Task<ActionResult<ApiResponse>> UpdateCuentaMadre([FromBody] Cuentas_Madre cuenta)
    {


        var response = await _cuentasxSubCuentasService.UpdateCuentaMadre(cuenta);
        if (!response.Success)
        {
            return BadRequest(response);
        }
        return Ok(response);
    }
    [HttpPut("UpdateCuentaTitulo", Name = "UpdateCuentaTitulo")]
    public async Task<ActionResult<ApiResponse>> UpdateCuentaTitulo([FromBody] Cuentas_Generales cuenta)
    {


        var response = await _cuentasxSubCuentasService.UpdateCuentaTitulo(cuenta);
        if (!response.Success)
        {
            return BadRequest(response);
        }
        return Ok(response);
    }
    [HttpPut("{id}")]
    public async Task<ActionResult<ApiResponse>> Update(int id, [FromBody] CuentasxSubCuentas cuentasxSubCuentas)
    {
        if (id != cuentasxSubCuentas.Id)
        {
            return BadRequest(ApiResponse.BadRequest("ID mismatch"));
        }

        var response = await _cuentasxSubCuentasService.UpdateCuentasxSubCuentasAsync(cuentasxSubCuentas);
        if (!response.Success)
        {
            return BadRequest(response);
        }
        return Ok(response);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<ApiResponse>> Delete(int id)
    {
        var response = await _cuentasxSubCuentasService.DeleteCuentasxSubCuentasAsync(id);
        if (!response.Success)
        {
            return BadRequest(response);
        }
        return Ok(response);
    }

    [HttpDelete("BorrarRelacion/{id}", Name = "BorrarRelacion")]
    public async Task<ActionResult<ApiResponse>> BorrarRelacion(int id)
    {


        var response = await _cuentasxSubCuentasService.BorrarRelacion(id);
        if (!response.Success)
        {
            return BadRequest(response);
        }
        return Ok(response);
    }

}
