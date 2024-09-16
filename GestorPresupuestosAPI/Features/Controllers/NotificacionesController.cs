 using GestorPresupuestosAPI.Features.Utility;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class NotificacionesController : ControllerBase
{
    private readonly NotificacionesService _notificacionesService;

    public NotificacionesController(NotificacionesService notificacionesService)
    {
        _notificacionesService = notificacionesService;
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse>> GetAllNotificaciones()
    {
        var response = await _notificacionesService.GetAllNotificacionesAsync();
        return Ok(response);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse>> GetNotificacionById(int id)
    {
        var response = await _notificacionesService.GetNotificacionByIdAsync(id);
        if (!response.Success)
        {
            return NotFound(response);
        }
        return Ok(response);
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse>> AddNotificacion([FromBody] Notificaciones notificacion)
    {
        var response = await _notificacionesService.AddNotificacionAsync(notificacion);
        return CreatedAtAction(nameof(GetNotificacionById), new { id = ((Notificaciones)response.Data).IdNoti }, response);
    }



    [HttpDelete("{id}")]
    public async Task<ActionResult<ApiResponse>> DeleteNotificacion(int id)
    {
        var response = await _notificacionesService.DeleteNotificacionAsync(id);
        if (!response.Success)
        {
            return NotFound(response);
        }
        return Ok(response);
    }

    [HttpPost("marcarLeidas")]
    public async Task<ActionResult<ApiResponse>> MarkNotificacionesAsLeida([FromBody] List<int> ids)
    {
        if (ids == null || ids.Count == 0)
        {
            return BadRequest(ApiResponse.BadRequest("Invalid request: no IDs provided."));
        }

        var response = await _notificacionesService.MarkNotificacionesAsLeidaAsync(ids);
        if (!response.Success)
        {
            return BadRequest(response);
        }

        return Ok(response);
    }

}
