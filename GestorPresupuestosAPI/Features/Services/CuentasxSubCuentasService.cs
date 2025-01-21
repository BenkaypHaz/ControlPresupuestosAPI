using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GestorPresupuestosAPI.Features.Repository;
using GestorPresupuestosAPI.Features.Utility;
using GestorPresupuestosAPI.Infraestructure.Entities;

public class CuentasxSubCuentasService
{
    private readonly CuentasxSubCuentasRepository _cuentasxSubCuentasRepository;

    public CuentasxSubCuentasService(CuentasxSubCuentasRepository cuentasxSubCuentasRepository)
    {
        _cuentasxSubCuentasRepository = cuentasxSubCuentasRepository;
    }

    public async Task<ApiResponse> GetAllCuentasxSubCuentasAsync()
    {
        try
        {
            var data = await _cuentasxSubCuentasRepository.GetAllCuentasxSubCuentasAsync();
            return ApiResponse.Ok("CuentasxSubCuentas retrieved successfully", data);
        }
        catch (Exception ex)
        {
            return ApiResponse.BadRequest($"Error retrieving data: {ex.Message}");
        }
    }
    public async Task<ApiResponse> GetCuentasMadre()
    {
        try
        {
            var data = await _cuentasxSubCuentasRepository.GetCuentasMadre();
            return ApiResponse.Ok("CuentasxSubCuentas retrieved successfully", data);
        }
        catch (Exception ex)
        {
            return ApiResponse.BadRequest($"Error retrieving data: {ex.Message}");
        }
    }
  
    public async Task<ApiResponse> GetCuentasTitulo()
    {
        try
        {
            var data = await _cuentasxSubCuentasRepository.GetCuentasTitulo();
            return ApiResponse.Ok("CuentasTitulo retrieved successfully", data);
        }
        catch (Exception ex)
        {
            return ApiResponse.BadRequest($"Error retrieving data: {ex.Message}");
        }
    }

    public async Task<ApiResponse> GetSubCuentasByIdCuenta(int id)
    {
        try
        {
            var data = await _cuentasxSubCuentasRepository.GetSubCuentasByIdCuenta(id);
            if (data == null)
            {
                return ApiResponse.NotFound($"CuentasxSubCuentas with ID {id} not found.");
            }
            return ApiResponse.Ok("CuentasxSubCuentas retrieved successfully", data);
        }
        catch (Exception ex)
        {
            return ApiResponse.BadRequest($"Error retrieving data: {ex.Message}");
        }
    }
    public async Task<ApiResponse> GetCuentasMadreById(int id)
    {
        try
        {
            var data = await _cuentasxSubCuentasRepository.GetCuentasMadreById(id);
            if (data == null)
            {
                return ApiResponse.NotFound($"CuentasxSubCuentas with ID {id} not found.");
            }
            return ApiResponse.Ok("CuentasxSubCuentas retrieved successfully", data);
        }
        catch (Exception ex)
        {
            return ApiResponse.BadRequest($"Error retrieving data: {ex.Message}");
        }
    }

    public async Task<ApiResponse> GetCuentasxSubCuentasByIdAsync(int id)
    {
        try
        {
            var data = await _cuentasxSubCuentasRepository.GetCuentasxSubCuentasByIdAsync(id);
            if (data == null)
            {
                return ApiResponse.NotFound($"CuentasxSubCuentas with ID {id} not found.");
            }
            return ApiResponse.Ok("CuentasxSubCuentas retrieved successfully", data);
        }
        catch (Exception ex)
        {
            return ApiResponse.BadRequest($"Error retrieving data: {ex.Message}");
        }
    }

    public async Task<ApiResponse> AddCuentaMadre(Cuentas_Madre data)
    {
        try
        {
            var createdEntity = await _cuentasxSubCuentasRepository.AddCuentaMadre(data);
            return ApiResponse.Ok("CuentasxSubCuentas added successfully", createdEntity);
        }
        catch (Exception ex)
        {
            return ApiResponse.BadRequest($"Error adding data: {ex.Message}");
        }
    }

    public async Task<ApiResponse> AddCuentaTitulo(Cuentas_Generales data)
    {
        try
        {
            var createdEntity = await _cuentasxSubCuentasRepository.AddCuentaTitulo(data);
            return ApiResponse.Ok("CuentasxSubCuentas added successfully", createdEntity);
        }
        catch (Exception ex)
        {
            return ApiResponse.BadRequest($"Error adding data: {ex.Message}");
        }
    }

    public async Task<ApiResponse> AddCuentasxSubCuentasAsync(CuentasxSubCuentas cuentasxSubCuentas)
    {
        try
        {
            var createdEntity = await _cuentasxSubCuentasRepository.AddCuentasxSubCuentasAsync(cuentasxSubCuentas);
            return ApiResponse.Ok("CuentasxSubCuentas added successfully", createdEntity);
        }
        catch (Exception ex)
        {
            return ApiResponse.BadRequest($"Error adding data: {ex.Message}");
        }
    }

    public async Task<ApiResponse> UpdateCuentaMadre(Cuentas_Madre cuenta)
    {
        try
        {
            await _cuentasxSubCuentasRepository.UpdateCuentaMadre(cuenta);
            return ApiResponse.Ok($"Cuenta with ID {cuenta.IdCuenta} updated successfully.");
        }
        catch (Exception ex)
        {
            return ApiResponse.BadRequest($"An error occurred while updating the cuenta: {ex.Message}");
        }
    }

    public async Task<ApiResponse> UpdateCuentaTitulo(Cuentas_Generales cuenta)
    {
        try
        {
            await _cuentasxSubCuentasRepository.UpdateCuentaTitulo(cuenta);
            return ApiResponse.Ok($"Cuenta with ID {cuenta.IdCuenta} updated successfully.");
        }
        catch (Exception ex)
        {
            return ApiResponse.BadRequest($"An error occurred while updating the cuenta: {ex.Message}");
        }
    }

    public async Task<ApiResponse> UpdateCuentasxSubCuentasAsync(CuentasxSubCuentas cuentasxSubCuentas)
    {
        try
        {
            await _cuentasxSubCuentasRepository.UpdateCuentasxSubCuentasAsync(cuentasxSubCuentas);
            return ApiResponse.Ok("CuentasxSubCuentas updated successfully");
        }
        catch (Exception ex)
        {
            return ApiResponse.BadRequest($"Error updating data: {ex.Message}");
        }
    }

    public async Task<ApiResponse> DeleteCuentasxSubCuentasAsync(int id)
    {
        try
        {
            await _cuentasxSubCuentasRepository.DeleteCuentasxSubCuentasAsync(id);
            return ApiResponse.Ok("CuentasxSubCuentas deleted successfully");
        }
        catch (Exception ex)
        {
            return ApiResponse.BadRequest($"Error deleting data: {ex.Message}");
        }
    }

    public async Task<ApiResponse> BorrarRelacion(int id)
    {
        try
        {
            await _cuentasxSubCuentasRepository.BorrarRelacion(id);
            return ApiResponse.Ok("CuentasxSubCuentas deleted successfully");
        }
        catch (Exception ex)
        {
            return ApiResponse.BadRequest($"Error deleting data: {ex.Message}");
        }
    }

}
