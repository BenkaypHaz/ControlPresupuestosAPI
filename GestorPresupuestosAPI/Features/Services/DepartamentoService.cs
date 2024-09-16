using GestorPresupuestosAPI.Features.Utility;
using GestorPresupuestosAPI.Infraestructure.Modelos;
using System;
using System.Threading.Tasks;

public class DepartamentoService
{
    private readonly DepartamentoRepository _departamentoRepository;

    public DepartamentoService(DepartamentoRepository departamentoRepository)
    {
        _departamentoRepository = departamentoRepository;
    }

    public async Task<ApiResponse> GetAllDepartamentosAsync()
    {
        try
        {
            var departamentos = await _departamentoRepository.GetAllDepartamentosAsync();
            return ApiResponse.Ok("Departamentos retrieved successfully", departamentos);
        }
        catch (Exception ex)
        {
            return ApiResponse.BadRequest($"An error occurred: {ex.Message}");
        }
    }

    public async Task<ApiResponse> AddDepartamentoAsync(Departamentos departamento)
    {
        try
        {
            var createdDepartamento = await _departamentoRepository.AddDepartamentoAsync(departamento);
            return ApiResponse.Ok("Departamento added successfully", createdDepartamento);
        }
        catch (Exception ex)
        {
            return ApiResponse.BadRequest($"An error occurred while adding the departamento: {ex.Message}");
        }
    }

    public async Task<ApiResponse> GetDepartamentoByIdAsync(int id)
    {
        try
        {
            var departamento = await _departamentoRepository.GetDepartamentoByIdAsync(id);
            if (departamento == null)
            {
                return ApiResponse.NotFound($"Departamento with ID {id} not found.");
            }
            return ApiResponse.Ok("Departamento found", departamento);
        }
        catch (Exception ex)
        {
            return ApiResponse.BadRequest($"An error occurred: {ex.Message}");
        }
    }

    public async Task<ApiResponse> UpdateDepartamentoAsync(int id,Departamentos departamento)
    {
        try
        {
            var existingDepartamento = await _departamentoRepository.GetDepartamentoByIdAsync(id);
            if (existingDepartamento == null)
            {
                return ApiResponse.NotFound($"Departamento with ID {departamento.IdDepartamento} not found.");
            }

            await _departamentoRepository.UpdateDepartamentoAsync(id,departamento);
            return ApiResponse.Ok($"Departamento with ID {id} updated successfully.");
        }
        catch (Exception ex)
        {
            return ApiResponse.BadRequest($"An error occurred while updating the departamento: {ex.Message}");
        }
    }

    public async Task<ApiResponse> DeleteDepartamentoAsync(int id)
    {
        try
        {
            var departamento = await _departamentoRepository.GetDepartamentoByIdAsync(id);
            if (departamento == null)
            {
                return ApiResponse.NotFound($"Departamento with ID {id} not found.");
            }

            await _departamentoRepository.DeleteDepartamentoAsync(id);
            return ApiResponse.Ok($"Departamento with ID {id} deleted successfully.");
        }
        catch (Exception ex)
        {
            return ApiResponse.BadRequest($"An error occurred while deleting the departamento: {ex.Message}");
        }
    }
}
