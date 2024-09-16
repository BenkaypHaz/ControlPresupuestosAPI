using GestorPresupuestosAPI.Features.Repository;
using GestorPresupuestosAPI.Features.Utility;

namespace GestorPresupuestosAPI.Features.Services
{
    public class DashboardService
    {
        private readonly DashboardRepository _dashboardRepository;
        public DashboardService(DashboardRepository dashboardRepository)
        {
            _dashboardRepository = dashboardRepository;
        }

        public async Task<ApiResponse> GetSummaryTipoPresu(int tipo)
        {
            try
            {
                var presupuestoCuenta = await _dashboardRepository.GetSummaryPresupuestoByTipo(tipo);
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

        public async Task<ApiResponse> GetSummaryByIdDepto(int idDepto,int tipoPresupuesto)
        {
            try
            {
                var presupuestoCuenta = await _dashboardRepository.GetPresupuestoSummaryByDepartment(idDepto, tipoPresupuesto);
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

        public async Task<ApiResponse> GetDepartamentoPresupuestoSummaryAsync()
        {
            try
            {
                var result = await _dashboardRepository.GetDepartamentoPresupuestoSummaryAsync();

                if (result == null || result.Count == 0)
                {
                    return ApiResponse.NotFound("No records found for the given presupuesto.");
                }

                return ApiResponse.Ok("Presupuesto summary retrieved successfully", result);
            }
            catch (Exception ex)
            {
                return ApiResponse.BadRequest($"An error occurred while retrieving the presupuesto summary: {ex.Message}");
            }
        }

    }
}
