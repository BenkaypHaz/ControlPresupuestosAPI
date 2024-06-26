using GestorPresupuestosAPI.Features.Utility;
using GestorPresupuestosAPI.Infraestructure.Modelos;
using System;
using System.Threading.Tasks;

public class SubcategoriaService
{
    private readonly SubcategoriaRepository _subcategoriaRepository;

    public SubcategoriaService(SubcategoriaRepository subcategoriaRepository)
    {
        _subcategoriaRepository = subcategoriaRepository;
    }

    public async Task<ApiResponse> GetAllSubcategoriasAsync()
    {
        try
        {
            var subcategorias = await _subcategoriaRepository.GetAllSubcategoriasAsync();
            return ApiResponse.Ok("Subcategorias retrieved successfully", subcategorias);
        }
        catch (Exception ex)
        {
            return ApiResponse.BadRequest($"An error occurred: {ex.Message}");
        }
    }

    public async Task<ApiResponse> AddSubcategoriaAsync(Subcategorias subcategoria)
    {
        try
        {
            var createdSubcategoria = await _subcategoriaRepository.AddSubcategoriaAsync(subcategoria);
            return ApiResponse.Ok("Subcategoria added successfully", createdSubcategoria);
        }
        catch (Exception ex)
        {
            return ApiResponse.BadRequest($"An error occurred while adding the subcategoria: {ex.Message}");
        }
    }

    public async Task<ApiResponse> GetSubcategoriaByIdAsync(int id)
    {
        try
        {
            var subcategoria = await _subcategoriaRepository.GetSubcategoriaByIdAsync(id);
            if (subcategoria == null)
            {
                return ApiResponse.NotFound($"Subcategoria with ID {id} not found.");
            }
            return ApiResponse.Ok("Subcategoria found", subcategoria);
        }
        catch (Exception ex)
        {
            return ApiResponse.BadRequest($"An error occurred: {ex.Message}");
        }
    }

    public async Task<ApiResponse> UpdateSubcategoriaAsync(Subcategorias subcategoria)
    {
        try
        {
            var existingSubcategoria = await _subcategoriaRepository.GetSubcategoriaByIdAsync(subcategoria.IdSubcate);
            if (existingSubcategoria == null)
            {
                return ApiResponse.NotFound($"Subcategoria with ID {subcategoria.IdSubcate} not found.");
            }

            await _subcategoriaRepository.UpdateSubcategoriaAsync(subcategoria);
            return ApiResponse.Ok($"Subcategoria with ID {subcategoria.IdSubcate} updated successfully.");
        }
        catch (Exception ex)
        {
            return ApiResponse.BadRequest($"An error occurred while updating the subcategoria: {ex.Message}");
        }
    }

    public async Task<ApiResponse> DeleteSubcategoriaAsync(int id)
    {
        try
        {
            var subcategoria = await _subcategoriaRepository.GetSubcategoriaByIdAsync(id);
            if (subcategoria == null)
            {
                return ApiResponse.NotFound($"Subcategoria with ID {id} not found.");
            }

            await _subcategoriaRepository.DeleteSubcategoriaAsync(id);
            return ApiResponse.Ok($"Subcategoria with ID {id} deleted successfully.");
        }
        catch (Exception ex)
        {
            return ApiResponse.BadRequest($"An error occurred while deleting the subcategoria: {ex.Message}");
        }
    }
}
