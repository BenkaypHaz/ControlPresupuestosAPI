using GestorPresupuestosAPI.Features.Utility;
using GestorPresupuestosAPI.Infraestructure.Modelos;
using System;
using System.Threading.Tasks;

public class CategoriaService
{
    private readonly CategoriaRepository _categoriaRepository;

    public CategoriaService(CategoriaRepository categoriaRepository)
    {
        _categoriaRepository = categoriaRepository;
    }

    public async Task<ApiResponse> GetAllCategoriasAsync()
    {
        try
        {
            var categorias = await _categoriaRepository.GetAllCategoriasAsync();
            return ApiResponse.Ok("Categorias retrieved successfully", categorias);
        }
        catch (Exception ex)
        {
            return ApiResponse.BadRequest($"An error occurred: {ex.Message}");
        }
    }

    public async Task<ApiResponse> AddCategoriaAsync(Categorias categoria)
    {
        try
        {
            var createdCategoria = await _categoriaRepository.AddCategoriaAsync(categoria);
            return ApiResponse.Ok("Categoria added successfully", createdCategoria);
        }
        catch (Exception ex)
        {
            return ApiResponse.BadRequest($"An error occurred while adding the categoria: {ex.Message}");
        }
    }

    public async Task<ApiResponse> GetCategoriaByIdAsync(int id)
    {
        try
        {
            var categoria = await _categoriaRepository.GetCategoriaByIdAsync(id);
            if (categoria == null)
            {
                return ApiResponse.NotFound($"Categoria with ID {id} not found.");
            }
            return ApiResponse.Ok("Categoria found", categoria);
        }
        catch (Exception ex)
        {
            return ApiResponse.BadRequest($"An error occurred: {ex.Message}");
        }
    }

    public async Task<ApiResponse> UpdateCategoriaAsync(Categorias categoria)
    {
        try
        {
            var existingCategoria = await _categoriaRepository.GetCategoriaByIdAsync(categoria.IdCat);
            if (existingCategoria == null)
            {
                return ApiResponse.NotFound($"Categoria with ID {categoria.IdCat} not found.");
            }

            await _categoriaRepository.UpdateCategoriaAsync(categoria);
            return ApiResponse.Ok($"Categoria with ID {categoria.IdCat} updated successfully.");
        }
        catch (Exception ex)
        {
            return ApiResponse.BadRequest($"An error occurred while updating the categoria: {ex.Message}");
        }
    }

    public async Task<ApiResponse> DeleteCategoriaAsync(int id)
    {
        try
        {
            await _categoriaRepository.DeactivateCategoriaAsync(id);
            return ApiResponse.Ok($"Categoria with ID {id} deactivated successfully.");
        }
        catch (Exception ex)
        {
            return ApiResponse.BadRequest($"An error occurred while deactivating the categoria: {ex.Message}");
        }
    }
}
