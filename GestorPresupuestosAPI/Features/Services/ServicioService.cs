using GestorPresupuestosAPI.Features.Utility;
using GestorPresupuestosAPI.Infraestructure.Entities;

namespace GestorPresupuestosAPI.Features.Services
{
    public class ServicioService
    {
        private readonly ServicioRepository _servicioRepository;

        public ServicioService(ServicioRepository servicioRepository)
        {
            _servicioRepository = servicioRepository;
        }

        public async Task<ApiResponse> GetAllServiciosAsync()
        {
            try
            {
                var servicios = await _servicioRepository.GetAllServiciosAsync();
                return ApiResponse.Ok("Servicios retrieved successfully", servicios);
            }
            catch (Exception ex)
            {
                return ApiResponse.BadRequest($"An error occurred while retrieving servicios: {ex.Message}");
            }
        }

        public async Task<ApiResponse> GetServicioByIdAsync(int id)
        {
            try
            {
                var servicio = await _servicioRepository.GetServicioByIdAsync(id);
                if (servicio != null)
                {
                    return ApiResponse.Ok("Servicio retrieved successfully", servicio);
                }
                return ApiResponse.NotFound("Servicio not found");
            }
            catch (Exception ex)
            {
                return ApiResponse.BadRequest($"An error occurred while retrieving the servicio: {ex.Message}");
            }
        }

        public async Task<ApiResponse> AddServicioAsync(Servicio servicio)
        {
            try
            {
                var createdServicio = await _servicioRepository.AddServicioAsync(servicio);
                return ApiResponse.Ok("Servicio added successfully", createdServicio);
            }
            catch (Exception ex)
            {
                return ApiResponse.BadRequest($"An error occurred while adding the servicio: {ex.Message}");
            }
        }

        public async Task<ApiResponse> UpdateServicioAsync(int id,Servicio servicio)
        {
            try
            {
                var updatedServicio = await _servicioRepository.UpdateServicioAsync(id,servicio);
                if (updatedServicio != null)
                {
                    return ApiResponse.Ok("Servicio updated successfully", updatedServicio);
                }
                return ApiResponse.NotFound("Servicio not found");
            }
            catch (Exception ex)
            {
                return ApiResponse.BadRequest($"An error occurred while updating the servicio: {ex.Message}");
            }
        }

        public async Task<ApiResponse> DeleteServicioAsync(int id)
        {
            try
            {
                var isDeleted = await _servicioRepository.DeleteServicioAsync(id);
                if (isDeleted)
                {
                    return ApiResponse.Ok("Servicio deleted successfully");
                }
                return ApiResponse.NotFound("Servicio not found");
            }
            catch (Exception ex)
            {
                return ApiResponse.BadRequest($"An error occurred while deleting the servicio: {ex.Message}");
            }
        }
    }
}
