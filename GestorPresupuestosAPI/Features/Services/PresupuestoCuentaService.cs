using GestorPresupuestosAPI.Features.Utility;
using GestorPresupuestosAPI.Infraestructure.Modelos;
using System;
using System.Threading.Tasks;

public class PresupuestoCuentaService
{
    private readonly PresupuestoCuentaRepository _presupuestoCuentaRepository;

    public PresupuestoCuentaService(PresupuestoCuentaRepository presupuestoCuentaRepository)
    {
        _presupuestoCuentaRepository = presupuestoCuentaRepository;
    }

    public async Task<ApiResponse> GetAllPresupuestoCuentasAsync()
    {
        try
        {
            var presupuestoCuentas = await _presupuestoCuentaRepository.GetAllPresupuestoCuentasAsync();
            return ApiResponse.Ok("PresupuestoCuentas retrieved successfully", presupuestoCuentas);
        }
        catch (Exception ex)
        {
            return ApiResponse.BadRequest($"An error occurred: {ex.Message}");
        }
    }

    public async Task<ApiResponse> AddPresupuestoCuentaAsync(PresupuestoCuenta presupuestoCuenta)
    {
        try
        {
            var createdPresupuestoCuenta = await _presupuestoCuentaRepository.AddPresupuestoCuentaAsync(presupuestoCuenta);
            return ApiResponse.Ok("PresupuestoCuenta added successfully", createdPresupuestoCuenta);
        }
        catch (Exception ex)
        {
            return ApiResponse.BadRequest($"An error occurred while adding the presupuestoCuenta: {ex.Message}");
        }
    }
    public async Task<List<CuentaDetailDTO>> GetAllCuentasDescriptionsAsync()
    {
        return await _presupuestoCuentaRepository.GetAllCuentas();
    }
    public async Task<ApiResponse> GetAllCuentasById(int idPresu)
    {
        try
        {
            var info = await _presupuestoCuentaRepository.GetAllCuentasById(idPresu);
            return ApiResponse.Ok("Info finded succesfully", info);
        }
        catch (Exception ex)
        {
            return ApiResponse.BadRequest($"An error occurred while searching cuentas: {ex.Message}");
        }
    }
    public async Task<ApiResponse> UpdatePresupuestoCuentaAsync(PresupuestoCuentaUpdateDTO dto)
    {
        try
        {
            await _presupuestoCuentaRepository.UpdatePresupuestoCuentaAsync(dto);
            return ApiResponse.Ok("Presupuesto cuenta updated successfully.");
        }
        catch (Exception ex)
        {
            return ApiResponse.BadRequest("Failed to update presupuesto cuenta.", new List<string> { ex.Message });
        }
    }
    public async Task<PresupuestoSummaryDTO> GetPresupuestoSummaryAsync()
    {
        return await _presupuestoCuentaRepository.GetPresupuestoSummaryAsync();
    }
    public async Task<ApiResponse> GetSummaryTipoPresu(int tipo)
    {
        try
        {
            var presupuestoCuenta = await _presupuestoCuentaRepository.GetSummaryPresupuestoByTipo(tipo);
            if (presupuestoCuenta == null)
            {
                return ApiResponse.NotFound($"Summary not found.");
            }
            return ApiResponse.Ok("Summary found", presupuestoCuenta);
        }
        catch (Exception ex)
        {
            return ApiResponse.BadRequest($"An error occurred searching Summary: {ex.Message}");
        }
    }
    public async Task<ApiResponse> GetSummaryAllPresu()
    {
        try
        {
            var presupuestoCuenta = await _presupuestoCuentaRepository.GetSummaryPresuEgreso();
            if (presupuestoCuenta == null)
            {
                return ApiResponse.NotFound($"Summary not found.");
            }
            return ApiResponse.Ok("Summary found", presupuestoCuenta);
        }
        catch (Exception ex)
        {
            return ApiResponse.BadRequest($"An error occurred searching Summary: {ex.Message}");
        }
    }
    public async Task<ApiResponse> GetPresupuestoSummaryById(int idPresu)
    {
        try
        {
            var presupuestoCuenta = await _presupuestoCuentaRepository.GetPresupuestoSummaryById(idPresu);
            if (presupuestoCuenta == null)
            {
                return ApiResponse.NotFound($"Summary with ID {idPresu} not found.");
            }
            return ApiResponse.Ok("Summary found", presupuestoCuenta);
        }
        catch (Exception ex)
        {
            return ApiResponse.BadRequest($"An error occurred searching Summary: {ex.Message}");
        }
    }
    public async Task<ApiResponse> GetSummaryByIdDepto(int idDepto)
    {
        try
        {
            var presupuestoCuenta = await _presupuestoCuentaRepository.GetPresupuestoSummaryByDepartment(idDepto);
            if (presupuestoCuenta == null)
            {
                return ApiResponse.NotFound($"Summary with ID not found.");
            }
            return ApiResponse.Ok("Summary found", presupuestoCuenta);
        }
        catch (Exception ex)
        {
            return ApiResponse.BadRequest($"An error occurred searching Summary: {ex.Message}");
        }
    }
    public async Task<ApiResponse> GetPresupuestoCuentaByIdAsync(int id)
    {
        try
        {
            var presupuestoCuenta = await _presupuestoCuentaRepository.GetPresupuestoCuentaByIdAsync(id);
            if (presupuestoCuenta == null)
            {
                return ApiResponse.NotFound($"PresupuestoCuenta with ID {id} not found.");
            }
            return ApiResponse.Ok("PresupuestoCuenta found", presupuestoCuenta);
        }
        catch (Exception ex)
        {
            return ApiResponse.BadRequest($"An error occurred: {ex.Message}");
        }
    }

    public async Task<ApiResponse> UpdatePresupuestoCuentaAsync(PresupuestoCuenta presupuestoCuenta)
    {
        try
        {
            var existingPresupuestoCuenta = await _presupuestoCuentaRepository.GetPresupuestoCuentaByIdAsync(presupuestoCuenta.IdCuentas);
            if (existingPresupuestoCuenta == null)
            {
                return ApiResponse.NotFound($"PresupuestoCuenta with ID {presupuestoCuenta.IdCuentas} not found.");
            }

            await _presupuestoCuentaRepository.UpdatePresupuestoCuentaAsync(presupuestoCuenta);
            return ApiResponse.Ok($"PresupuestoCuenta with ID {presupuestoCuenta.IdCuentas} updated successfully.");
        }
        catch (Exception ex)
        {
            return ApiResponse.BadRequest($"An error occurred while updating the presupuestoCuenta: {ex.Message}");
        }
    }

    public async Task<ApiResponse> DeletePresupuestoCuentaAsync(int id)
    {
        try
        {
            await _presupuestoCuentaRepository.DeletePresupuestoCuentaAsync(id);
            return ApiResponse.Ok($"PresupuestoCuenta with ID {id} deleted successfully.");
        }
        catch (Exception ex)
        {
            return ApiResponse.BadRequest($"An error occurred while deleting the presupuestoCuenta: {ex.Message}");
        }
    }

    public async Task<ApiResponse> DesactivarEjeParcial(int id)
    {
        try
        {
            await _presupuestoCuentaRepository.DesactivarEjeParcial(id);
            return ApiResponse.Ok($"PresupuestoCuenta with ID {id} desactivated successfully.");
        }
        catch (Exception ex)
        {
            return ApiResponse.BadRequest($"An error occurred while deleting the presupuestoCuenta: {ex.Message}");
        }
    }

    public async Task<ApiResponse> DesactivarEjecucion(int id, bool esParcial)
    {
        try
        {
            await _presupuestoCuentaRepository.DesactivarEjecucion(id, esParcial);
            return ApiResponse.Ok($"Ejecucion with ID {id} desactivated successfully.");
        }
        catch (Exception ex)
        {
            return ApiResponse.BadRequest($"An error occurred while deleting the presupuestoCuenta: {ex.Message}");
        }
    }

    public async Task<ApiResponse> AddItemsEjecutadosAsync(ItemsEjecutados item)
    {
        try
        {
            await _presupuestoCuentaRepository.AddItemsEjecutadosAsync(item);
            return ApiResponse.Ok("Item added successfully", item);
        }
        catch (Exception ex)
        {
            return ApiResponse.BadRequest($"An error occurred: {ex.Message}");
        }
    }

    public async Task<ApiResponse> AddItemsEjecutadosAdminAsync(ItemsConNoti item)
    {
        try
        {
            await _presupuestoCuentaRepository.AddItemsEjecutadoAdminsAsync(item.Item, item.UsuModifica, item.Comentario);
            return ApiResponse.Ok("Item added successfully", item);
        }
        catch (Exception ex)
        {
            return ApiResponse.BadRequest($"An error occurred: {ex.Message}");
        }
    }

    public async Task<ApiResponse> AddItemsEjecucionParcialAdminAsync(ItemsParcialesConNoti item)
    {
        try
        {
            await _presupuestoCuentaRepository.AddItemsEjecucionParcialAdminAsync(item.Items, item.UsuModifica, item.Comentario);
            return ApiResponse.Ok("Item added successfully", item);
        }
        catch (Exception ex)
        {
            return ApiResponse.BadRequest($"An error occurred: {ex.Message}");
        }
    }
    public async Task<ApiResponse> AddItemsEjecucionParcialAsync(ItemsEjecucionParcial item)
    {
        try
        {
            await _presupuestoCuentaRepository.AddItemsEjecucionParcialAsync(item);
            return ApiResponse.Ok("Item added successfully", item);
        }
        catch (Exception ex)
        {
            return ApiResponse.BadRequest($"An error occurred: {ex.Message}");
        }
    }

    public async Task<ApiResponse> FindItemCuentaResumen(int id)
    {
        try
        {
            var resumen = await _presupuestoCuentaRepository.FindItemCuentaResumed(id);
            if (resumen == null)
            {
                return ApiResponse.NotFound($"PresupuestoCuentaResumen with ID {id} not found.");
            }

            return ApiResponse.Ok($"PresupuestoCuenta with ID {id} updated successfully.", resumen);
        }
        catch (Exception ex)
        {
            return ApiResponse.BadRequest($"An error occurred while searching the presupuestoCuenta: {ex.Message}");
        }
    }


}
