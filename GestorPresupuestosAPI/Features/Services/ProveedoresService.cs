using GestorPresupuestosAPI.Features.Repository;
using GestorPresupuestosAPI.Features.Utility;
using GestorPresupuestosAPI.Infraestructure.Entities;

namespace GestorPresupuestosAPI.Features.Services
{
    public class ProveedoresService
    {
        private readonly ProveedoresRepository _proveedoresRepository;

        public ProveedoresService(ProveedoresRepository proveedoresRepository)
        {
            _proveedoresRepository = proveedoresRepository;
        }

        public async Task<ApiResponse> GetAllProveedoresAsync()
        {
            try
            {
                var cuentas = await _proveedoresRepository.GetAllProveedoresAsync();
                return ApiResponse.Ok("Proveedores retrieved successfully", cuentas);
            }
            catch (Exception ex)
            {
                return ApiResponse.BadRequest($"An error occurred: {ex.Message}");
            }
        }

        public async Task<Proveedores> AddProveedorAsync(Proveedores proveedor)
        {
            return await _proveedoresRepository.AddAsync(proveedor);
        }

        public async Task<ApiResponse> UpdateProveedorAsync(Proveedores proveedor)
        {
            try
            {
                await _proveedoresRepository.UpdateAsync(proveedor);
                return ApiResponse.Ok("Proveedor updated successfully.");
            }
            catch (Exception ex)
            {
                return ApiResponse.BadRequest($"An error occurred while updating the proveedor: {ex.Message}");
            }
        }

        public async Task<ApiResponse> DeleteProveedorAsync(int id)
        {
            try
            {
                await _proveedoresRepository.DeleteAsync(id);
                return ApiResponse.Ok("Proveedor deleted successfully.");
            }
            catch (Exception ex)
            {
                return ApiResponse.BadRequest($"An error occurred while deleting the proveedor: {ex.Message}");
            }
        }


    }
}
