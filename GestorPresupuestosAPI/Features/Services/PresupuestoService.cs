using GestorPresupuestosAPI.Features.Utility;
using GestorPresupuestosAPI.Infraestructure.DataBases;
using GestorPresupuestosAPI.Infraestructure.Modelos;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

public class PresupuestoService
{
    private readonly PresupuestoRepository _presupuestoRepository;
    private readonly GestorPresupuestosAHM _context;
    private readonly PresupuestoCuentaRepository _presupuestoCuentaRepository;

    public PresupuestoService(GestorPresupuestosAHM context,PresupuestoRepository presupuestoRepository, PresupuestoCuentaRepository presupuestoCuentaRepository)
    {
        _presupuestoRepository = presupuestoRepository;
        _context = context;
        _presupuestoCuentaRepository = presupuestoCuentaRepository;

    }

    public async Task<ApiResponse> GetAllPresupuestosAsync()
    {
        try
        {
            var presupuestos = await _presupuestoRepository.GetAllPresupuestosAsync();
            return ApiResponse.Ok("Presupuestos retrieved successfully", presupuestos);
        }
        catch (Exception ex)
        {
            return ApiResponse.BadRequest($"An error occurred: {ex.Message}");
        }
    }
    public async Task<ApiResponse> GetPresupuestoBydId(int idPresu)
    {
        try
        {
            var presupuestos = await _presupuestoRepository.GetPresupuestoById(idPresu);
            return ApiResponse.Ok("Presupuestos retrieved successfully", presupuestos);
        }
        catch (Exception ex)
        {
            return ApiResponse.BadRequest($"An error occurred: {ex.Message}");
        }
    }
    public async Task<ApiResponse> GetAllPresupuestosAdminAsync()
    {
        try
        {
            var presupuestos = await _presupuestoRepository.GetAllPresupuestosAdminAsync();
            return ApiResponse.Ok("Presupuestos retrieved successfully", presupuestos);
        }
        catch (Exception ex)
        {
            return ApiResponse.BadRequest($"An error occurred: {ex.Message}");
        }
    }
    public async Task<ApiResponse> GetPresupuestoByUsuId(int id, int rol)
    {
        try
        {
            var presupuestos = await _presupuestoRepository.GetPresupuestoByUsuId(id, rol);
            return ApiResponse.Ok("Presupuestos retrieved successfully", presupuestos);
        }
        catch (Exception ex)
        {
            return ApiResponse.BadRequest($"An error occurred: {ex.Message}");
        }
    }
    public async Task<ApiResponse> GetAllPresupuestosAprobadosAdminAsync()
    {
        try
        {
            var presupuestos = await _presupuestoRepository.GetAllPresupuestosAprobadosAdminAsync();
            return ApiResponse.Ok("Presupuestos retrieved successfully", presupuestos);
        }
        catch (Exception ex)
        {
            return ApiResponse.BadRequest($"An error occurred: {ex.Message}");
        }
    }
    public async Task<ApiResponse> GetTipoPresupuesto()
    {
        try
        {
            var presupuestos = await _presupuestoRepository.GetTipoPresupuesto();
            return ApiResponse.Ok("Presupuestos retrieved successfully", presupuestos);
        }
        catch (Exception ex)
        {
            return ApiResponse.BadRequest($"An error occurred: {ex.Message}");
        }
    }

    
    public async Task<ApiResponse> CrearPresupuestoAsync(string nombre, int tipo, int usuid, int anio)
    {
        try
        {
            var createdPresupuesto = await _presupuestoRepository.CrearPresupuesto(nombre,tipo,usuid, anio);
            return ApiResponse.Ok("Presupuesto added successfully", createdPresupuesto);
        }
        catch (Exception ex)
        {
            return ApiResponse.BadRequest($"An error occurred while adding the presupuesto: {ex.Message}");
        }
    }
    public async Task<ApiResponse> GetPresupuestoCreadoById(int id)
    {
        try
        {
            var presupuesto = await _presupuestoRepository.GetPresupuestoCreadoById(id);
            if (presupuesto == null)
            {
                return ApiResponse.NotFound($"Presupuesto with ID {id} not found.");
            }
            return ApiResponse.Ok("Presupuesto found", presupuesto);
        }
        catch (Exception ex)
        {
            return ApiResponse.BadRequest($"An error occurred: {ex.Message}");
        }
    }
    public async Task<ApiResponse> GetPresupuestoByUsuIdAsync(int id)
    {
        try
        {
            var presupuesto = await _presupuestoRepository.GetPresupuestoByUsuAsync(id);
            if (presupuesto == null)
            {
                return ApiResponse.NotFound($"Presupuesto with ID {id} not found.");
            }
            return ApiResponse.Ok("Presupuesto found", presupuesto);
        }
        catch (Exception ex)
        {
            return ApiResponse.BadRequest($"An error occurred: {ex.Message}");
        }
    }
    public async Task<ApiResponse> GetPresupuestosUsuDashboard(int id, int tipo, int anio)
    {
        try
        {
            var presupuesto = await _presupuestoRepository.GetPresupuestosUsuDashboard(id, tipo, anio);
            if (presupuesto == null)
            {
                return ApiResponse.NotFound($"Presupuesto with ID {id} not found.");
            }
            return ApiResponse.Ok("Presupuesto found", presupuesto);
        }
        catch (Exception ex)
        {
            return ApiResponse.BadRequest($"An error occurred: {ex.Message}");
        }
    }

    public async Task<ApiResponse> GetPresupuestoByIdAsync(int id)
    {
        try
        {
            var presupuesto = await _presupuestoRepository.GetPresupuestoByIdAsync(id);
            if (presupuesto == null || !presupuesto.Activo)
            {
                return ApiResponse.NotFound($"Presupuesto with ID {id} not found.");
            }
            return ApiResponse.Ok("Presupuesto found", presupuesto);
        }
        catch (Exception ex)
        {
            return ApiResponse.BadRequest($"An error occurred: {ex.Message}");
        }
    }

    public async Task<ApiResponse> UpdatePresupuestoAsync(Presupuesto presupuesto)
    {
        try
        {
            var existingPresupuesto = await _presupuestoRepository.GetPresupuestoByIdAsync(presupuesto.IdPresu);
            if (existingPresupuesto == null)
            {
                return ApiResponse.NotFound($"Presupuesto with ID {presupuesto.IdPresu} not found.");
            }

            await _presupuestoRepository.UpdatePresupuestoAsync(presupuesto);
            return ApiResponse.Ok($"Presupuesto with ID {presupuesto.IdPresu} updated successfully.");
        }
        catch (Exception ex)
        {
            return ApiResponse.BadRequest($"An error occurred while updating the presupuesto: {ex.Message}");
        }
    }

    public async Task<ApiResponse> UpdatePresupuestoEstado(int idPresu, int estado)
    {
        try
        {
            await _presupuestoRepository.UpdatePresupuestoEstado(idPresu, estado);
            return ApiResponse.Ok($"Presupuesto with ID {idPresu} updated successfully.");
        }
        catch (Exception ex)
        {
            return ApiResponse.BadRequest($"An error occurred while updating the presupuesto estado: {ex.Message}");
        }
    }
    public async Task<ApiResponse> UpdatePresupuestoEstadoAsync(int idPresu, int estado, int usuModifica)
    {
        try
        {
             await _presupuestoRepository.UpdatePresupuestoEstadoAsync(idPresu, estado, usuModifica);
             return ApiResponse.Ok($"Presupuesto with ID {idPresu} updated successfully.");
        }
        catch (Exception ex)
        {
            return ApiResponse.BadRequest($"An error occurred while updating the presupuesto estado: {ex.Message}");
        }
    }

    public async Task<ApiResponse> DeletePresupuestoAsync(int id)
    {
        try
        {
             await _presupuestoRepository.DeactivatePresupuestoAsync(id);
            return ApiResponse.Ok($"Presupuesto with ID {id} deactivated successfully.");
        }
        catch (Exception ex)
        {
            return ApiResponse.BadRequest($"An error occurred while deactivating the presupuesto: {ex.Message}");
        }
    }

    public async Task<ApiResponse> BloquearCuentasPresupuesto(int idPresu, DateTime fechainicio, DateTime fechafin)
    {
        try
        {
            await _presupuestoRepository.BloquearCuentasPresupuesto(idPresu, fechainicio, fechafin);
            return ApiResponse.Ok($"Cuentas Bloqueadas.");
        }
        catch (Exception ex)
        {
            return ApiResponse.BadRequest($"An error occurred while deactivating the presupuesto: {ex.Message}");
        }
    }

    public async Task<ApiResponse> GetPresupuestoInfo(int id)
    {
        try
        {
            PresupuestoWithCuentasDTO presu = await _presupuestoRepository.GetPresupuestoInfo(id);
            return ApiResponse.Ok("Information Found",presu);
        }
        catch (Exception ex)
        {
            return ApiResponse.BadRequest($"An error occurred while searching the presupuesto: {ex.Message}");
        }
    }


    public async Task<ApiResponse> AddPresupuestoWithCuentasAsync(decimal total, PresupuestoCuenta newCuenta)
    {
        using (var transaction = _context.Database.BeginTransaction())
        {
            try
            {

                await _presupuestoCuentaRepository.AddPresupuestoCuentasAsync(newCuenta);

                Presupuesto presupuesto = await _presupuestoRepository.AddPresupuestoAsync(total, newCuenta.IdPresu);


                transaction.Commit();
                return ApiResponse.Ok("Presupuesto and related cuentas added successfully", null);
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                return ApiResponse.BadRequest($"An error occurred: {ex.Message}");
            }
        }
    }



}
