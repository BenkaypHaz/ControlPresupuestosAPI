using GestorPresupuestosAPI.Features.Repository;
using GestorPresupuestosAPI.Features.Utility;

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

     
    }
}
