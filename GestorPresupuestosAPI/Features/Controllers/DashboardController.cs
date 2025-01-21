using GestorPresupuestosAPI.Features.Services;
using GestorPresupuestosAPI.Features.Utility;
using Microsoft.AspNetCore.Mvc;

namespace GestorPresupuestosAPI.Features.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly DashboardService _dashboardService;
        public DashboardController(DashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        [HttpGet("ResumenPorTipo/{tipopresu}/{anio}", Name = "ResumenPorTipo")]
        public async Task<ActionResult<ApiResponse>> GetSummaryTipoPresu(int tipopresu, int anio)
        {
            var response = await _dashboardService.GetSummaryTipoPresu(tipopresu,anio);
            if (!response.Success)
            {
                return NotFound(response);
            }
            return Ok(response);
        }

        [HttpGet("ResumenPorDepartamento", Name = "ResumenPorDepartamento")]
        public async Task<ActionResult<ApiResponse>> GetSummaryByIdDepto(int idDepto, int tipoPresupuesto)
        {
            var response = await _dashboardService.GetSummaryByIdDepto(idDepto, tipoPresupuesto);
            if (!response.Success)
            {
                return NotFound(response);
            }
            return Ok(response);
        }

        [HttpGet("datos-pastel/{anio}/{tipopresu}", Name = "datos-pastel")]
        public async Task<IActionResult> GetDepartamentoPresupuestoSummary(int anio,int tipopresu)
        {
            var result = await _dashboardService.GetDepartamentoPresupuestoSummaryAsync(anio, tipopresu);
            return Ok(result);
        }

        [HttpGet("StatsPresupuestoIndividual/{presuId}/{anio}", Name = "StatsPresupuestoIndividual")]
        public async Task<ActionResult<ApiResponse>> StatsPresupuestoIndividual(int presuId, int anio)
        {
            var response = await _dashboardService.GetStatsPresupuestoIndividual(presuId,anio);
            if (!response.Success)
            {
                return NotFound(response);
            }
            return Ok(response);
        }


        [HttpGet("GetStatsCuentasPresupuestadoAsync/{idPresu}/{anio}", Name = "GetStatsCuentasPresupuestadoAsync")]
        public async Task<ActionResult<ApiResponse>> GetStatsCuentasPresupuestadoAsync(int idPresu, int anio)
        {
            var response = await _dashboardService.GetStatsCuentasPresupuestadoAsync(idPresu,anio);
            if (!response.Success)
            {
                return NotFound(response);
            }
            return Ok(response);
        }

        [HttpGet("GetDepartamentoStatsPresupuestadoAsync/{year}/{tipoPresupuesto}", Name = "GetDepartamentoStatsPresupuestadoAsync")]
        public async Task<ActionResult<ApiResponse>> GetDepartamentoStatsPresupuestadoAsync(int year, int tipoPresupuesto)
        {
            var response = await _dashboardService.GetDepartamentoStatsPresupuestadoAsync(year, tipoPresupuesto);
            if (!response.Success)
            {
                return NotFound(response);
            }
            return Ok(response);
        }

        [HttpGet("GetPresupuestoAnualStatsAsync/{tipo}", Name = "GetPresupuestoAnualStatsAsync")]
        public async Task<ActionResult<ApiResponse>> GetPresupuestoAnualStatsAsync(int tipo)
        {
            var response = await _dashboardService.GetPresupuestoAnualStatsAsync(tipo);
            if (!response.Success)
            {
                return NotFound(response);
            }
            return Ok(response);
        }

    }
}
