using GestorPresupuestosAPI.Features.Utility;
using System;
using System.Threading.Tasks;

public class CuentaService
{
    private readonly CuentaRepository _cuentaRepository;

    public CuentaService(CuentaRepository cuentaRepository)
    {
        _cuentaRepository = cuentaRepository;
    }

    public async Task<ApiResponse> GetAllCuentasAsync()
    {
        try
        {
            var cuentas = await _cuentaRepository.GetAllCuentasAsync();
            return ApiResponse.Ok("Cuentas retrieved successfully", cuentas);
        }
        catch (Exception ex)
        {
            return ApiResponse.BadRequest($"An error occurred: {ex.Message}");
        }
    }

    public async Task<ApiResponse> AddCuentaAsync(Cuenta cuenta)
    {
        try
        {
            var createdCuenta = await _cuentaRepository.AddCuentaAsync(cuenta);
            return ApiResponse.Ok("Cuenta added successfully", createdCuenta);
        }
        catch (Exception ex)
        {
            return ApiResponse.BadRequest($"An error occurred while adding the cuenta: {ex.Message}");
        }
    }

    public async Task<ApiResponse> GetCuentaByIdAsync(int id)
    {
        try
        {
            var cuenta = await _cuentaRepository.GetCuentaByIdAsync(id);
            if (cuenta == null)
            {
                return ApiResponse.NotFound($"Cuenta with ID {id} not found.");
            }
            return ApiResponse.Ok("Cuenta found", cuenta);
        }
        catch (Exception ex)
        {
            return ApiResponse.BadRequest($"An error occurred: {ex.Message}");
        }
    }

    public async Task<ApiResponse> UpdateCuentaAsync(Cuenta cuenta)
    {
        try
        {
            var existingCuenta = await _cuentaRepository.GetCuentaByIdAsync(cuenta.IdCuenta);
            if (existingCuenta == null)
            {
                return ApiResponse.NotFound($"Cuenta with ID {cuenta.IdCuenta} not found.");
            }

            await _cuentaRepository.UpdateCuentaAsync(cuenta);
            return ApiResponse.Ok($"Cuenta with ID {cuenta.IdCuenta} updated successfully.");
        }
        catch (Exception ex)
        {
            return ApiResponse.BadRequest($"An error occurred while updating the cuenta: {ex.Message}");
        }
    }

    public async Task<ApiResponse> DeleteCuentaAsync(int id)
    {
        try
        {
            await _cuentaRepository.DeleteCuentaAsync(id);
            return ApiResponse.Ok($"Cuenta with ID {id} deleted successfully.");
        }
        catch (Exception ex)
        {
            return ApiResponse.BadRequest($"An error occurred while deleting the cuenta: {ex.Message}");
        }
    }
}
