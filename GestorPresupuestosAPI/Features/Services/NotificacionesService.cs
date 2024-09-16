using GestorPresupuestosAPI.Features.Utility;

public class NotificacionesService
{
    private readonly NotificacionesRepository _notificacionesRepository;

    public NotificacionesService(NotificacionesRepository notificacionesRepository)
    {
        _notificacionesRepository = notificacionesRepository;
    }

    public async Task<ApiResponse> GetAllNotificacionesAsync()
    {
        try
        {
            var notificaciones = await _notificacionesRepository.GetAllNotificacionesAsync();
            return ApiResponse.Ok("Notificaciones retrieved successfully.", notificaciones);
        }
        catch (Exception ex)
        {
            return ApiResponse.BadRequest("An error occurred while retrieving notificaciones.", new List<string> { ex.Message });
        }
    }

    public async Task<ApiResponse> GetNotificacionByIdAsync(int id)
    {
        try
        {
            var notificacion = await _notificacionesRepository.GetNotificacionByIdAsync(id);
            if (notificacion == null)
            {
                return ApiResponse.NotFound("Notificación not found.");
            }
            return ApiResponse.Ok("Notificación retrieved successfully.", notificacion);
        }
        catch (Exception ex)
        {
            return ApiResponse.BadRequest("An error occurred while retrieving the notificación.", new List<string> { ex.Message });
        }
    }

    public async Task<ApiResponse> MarkNotificacionesAsLeidaAsync(List<int> ids)
    {
        try
        {
            await _notificacionesRepository.MarkNotificacionesAsLeidaAsync(ids);
            return ApiResponse.Ok("Notificaciones marked as leída successfully.");
        }
        catch (Exception ex)
        {
            return ApiResponse.BadRequest("An error occurred while marking notificaciones as leída.", new List<string> { ex.Message });
        }
    }
    public async Task<ApiResponse> AddNotificacionAsync(Notificaciones notificacion)
    {
        try
        {
            var createdNotificacion = await _notificacionesRepository.AddNotificacionAsync(notificacion);
            return ApiResponse.Ok("Notificación added successfully.", createdNotificacion);
        }
        catch (Exception ex)
        {
            return ApiResponse.BadRequest("An error occurred while adding the notificación.", new List<string> { ex.Message });
        }
    }


    public async Task<ApiResponse> DeleteNotificacionAsync(int id)
    {
        try
        {
            var existingNotificacion = await _notificacionesRepository.GetNotificacionByIdAsync(id);
            if (existingNotificacion == null)
            {
                return ApiResponse.NotFound("Notificación not found.");
            }

            await _notificacionesRepository.DeleteNotificacionAsync(id);
            return ApiResponse.Ok("Notificación deleted successfully.");
        }
        catch (Exception ex)
        {
            return ApiResponse.BadRequest("An error occurred while deleting the notificación.", new List<string> { ex.Message });
        }
    }
}
