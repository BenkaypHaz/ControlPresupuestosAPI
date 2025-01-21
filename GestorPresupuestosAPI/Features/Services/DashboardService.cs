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

        public async Task<ApiResponse> GetSummaryTipoPresu(int tipo, int anio)
        {
            try
            {
                var presupuestoCuenta = await _dashboardRepository.GetSummaryPresupuestoByTipo(tipo,anio);
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

        public async Task<ApiResponse> GetDepartamentoPresupuestoSummaryAsync(int anio, int tipopresu)
        {
            try
            {
                var result = await _dashboardRepository.GetDepartamentoPresupuestoSummaryAsync(anio,tipopresu);

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

        public async Task<ApiResponse> GetStatsPresupuestoIndividual(int IdPresu, int anio)
        {
            try
            {
                var result = await _dashboardRepository.StatsPresupuestoIndividual(IdPresu,anio);

                if (result == null)
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

        public async Task<ApiResponse> GetStatsCuentasPresupuestadoAsync(int IdPresu, int anio)
        {
            try
            {
                var result = await _dashboardRepository.GetStatsCuentasPresupuestadoAsync(IdPresu,anio);

                if (result == null)
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

        public async Task<ApiResponse> GetDepartamentoStatsPresupuestadoAsync(int year, int tipoPresupuesto)
        {
            try
            {
                var result = await _dashboardRepository.GetDepartamentoStatsPresupuestadoAsync(year, tipoPresupuesto);

                if (result == null)
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

        public async Task<ApiResponse> GetPresupuestoAnualStatsAsync(int tipo)
        {
            try
            {
                var result = await _dashboardRepository.GetPresupuestoAnualStatsAsync(tipo);

                if (result == null)
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
