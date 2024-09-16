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

        [HttpGet("ResumenPorTipo/{tipopresu}", Name = "ResumenPorTipo")]
        public async Task<ActionResult<ApiResponse>> GetSummaryTipoPresu(int tipopresu)
        {
            var response = await _dashboardService.GetSummaryTipoPresu(tipopresu);
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

        [HttpGet("datos-pastel")]
        public async Task<IActionResult> GetDepartamentoPresupuestoSummary()
        {
            var result = await _dashboardService.GetDepartamentoPresupuestoSummaryAsync();
            return Ok(result);
        }
    }
}
