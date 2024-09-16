using GestorPresupuestosAPI.Features.Services;
using GestorPresupuestosAPI.Infraestructure.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

[Route("api/[controller]")]
[ApiController]
public class ServicioController : ControllerBase
{
    private readonly ServicioService _servicioService;

    public ServicioController(ServicioService servicioService)
    {
        _servicioService = servicioService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllServicios()
    {
        var response = await _servicioService.GetAllServiciosAsync();
        return Ok(response);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetServicioById(int id)
    {
        var response = await _servicioService.GetServicioByIdAsync(id);
        if (!response.Success)
        {
            return NotFound(response);
        }
        return Ok(response);
    }

    [HttpPost]
    public async Task<IActionResult> AddServicio([FromBody] Servicio servicio)
    {
        var response = await _servicioService.AddServicioAsync(servicio);
        if (!response.Success)
        {
            return BadRequest(response);
        }
        return Ok(response);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateServicio(int id, [FromBody] Servicio servicio)
    {
 

        var response = await _servicioService.UpdateServicioAsync(id,servicio);
        if (!response.Success)
        {
            return NotFound(response);
        }
        return Ok(response);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteServicio(int id)
    {
        var response = await _servicioService.DeleteServicioAsync(id);
        if (!response.Success)
        {
            return NotFound(response);
        }
        return Ok(response);
    }
}
