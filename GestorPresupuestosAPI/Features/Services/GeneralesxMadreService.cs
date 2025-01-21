using GestorPresupuestosAPI.Features.Repository;
using GestorPresupuestosAPI.Features.Utility;

namespace GestorPresupuestosAPI.Features.Services
{
    public class GeneralesxMadreService
    {
        private readonly GeneralesxMadreRepository _repository;

        public GeneralesxMadreService(GeneralesxMadreRepository repository)
        {
            _repository = repository;
        }

        public async Task<ApiResponse> GetAllAsync()
        {
            try
            {
                var result = await _repository.GetAllAsync();
                return ApiResponse.Ok("Data retrieved successfully", result);
            }
            catch (Exception ex)
            {
                return ApiResponse.BadRequest($"An error occurred while retrieving the data: {ex.Message}");
            }
        }

        public async Task<ApiResponse> GetByIdAsync(int id)
        {
            try
            {
                var result = await _repository.GetByIdAsync(id);
                if (result == null)
                {
                    return ApiResponse.NotFound($"Record with ID {id} not found.");
                }
                return ApiResponse.Ok("Record retrieved successfully", result);
            }
            catch (Exception ex)
            {
                return ApiResponse.BadRequest($"An error occurred while retrieving the record: {ex.Message}");
            }
        }

        public async Task<ApiResponse> AddAsync(GeneralesxMadre dto)
        {
            try
            {
                var entity = new GeneralesxMadre
                {
                    IdGeneral = dto.IdGeneral,
                    IdMadre = dto.IdMadre
                };

                var result = await _repository.AddAsync(entity);
                return ApiResponse.Ok("Record added successfully", result);
            }
            catch (Exception ex)
            {
                return ApiResponse.BadRequest($"An error occurred while adding the record: {ex.Message}");
            }
        }

        public async Task<ApiResponse> UpdateAsync(GeneralesxMadre dto)
        {
            try
            {
                var entity = new GeneralesxMadre
                {
                    Id = dto.Id,
                    IdGeneral = dto.IdGeneral,
                    IdMadre = dto.IdMadre
                };

                var result = await _repository.UpdateAsync(entity);
                return ApiResponse.Ok("Record updated successfully", result);
            }
            catch (Exception ex)
            {
                return ApiResponse.BadRequest($"An error occurred while updating the record: {ex.Message}");
            }
        }

        public async Task<ApiResponse> DeleteAsync(int id)
        {
            try
            {
                await _repository.DeleteAsync(id);
                return ApiResponse.Ok("Record deleted successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse.BadRequest($"An error occurred while deleting the record: {ex.Message}");
            }
        }
    }
}
