using GestorPresupuestosAPI.Features.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Reporting.WebForms;
using System.Threading.Tasks;

[Route("api/[controller]")]
[ApiController]
public class ReportController : ControllerBase
{
    private readonly ReportService _reportService;

    public ReportController(ReportService reportService)
    {
        _reportService = reportService;
    }

    [HttpGet("PresupuestoGeneral")]
    public async Task<IActionResult> GetPresupuestoGeneral([FromQuery] int idPresu)
    {
        try
        {
            var parameters = new ReportParameter[]
            {
                new ReportParameter("idPresu", idPresu.ToString())
            };

            var reportBytes = await _reportService.GenerateReportAsync("/Reports_GestorPresupuestos/PresupuestoGeneral", parameters);

            return File(reportBytes, "application/pdf", "PresupuestoGeneralReport.pdf");
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = ex.Message });
        }
    }

    [HttpGet("PresupuestoGeneralExcel")]
    public async Task<IActionResult> PresupuestoGeneralExcel([FromQuery] int idPresu)
    {
        try
        {
            var parameters = new ReportParameter[]
            {
            new ReportParameter("idPresu", idPresu.ToString())
            };

            // Call the GenerateReportExcelAsync for Excel export
            var reportBytes = await _reportService.GenerateReportExcelAsync("/Reports_GestorPresupuestos/PresupuestoGeneral", parameters);

            return File(reportBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "PresupuestoGeneralReport.xlsx");
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = ex.Message });
        }
    }

    [HttpGet("PresupuestoCuentaslExcel")]
    public async Task<IActionResult> PresupuestoCuentaslExcel([FromQuery] string nombre, int anio)
    {
        try
        {
            var parameters = new ReportParameter[]
            {
            new ReportParameter("anio", anio.ToString())
            };

            // Call the GenerateReportExcelAsync for Excel export
            var reportBytes = await _reportService.GenerateReportExcelAsync($"/Reports_GestorPresupuestos/{nombre}", parameters);

            return File(reportBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "PresupuestoGeneralReport.xlsx");
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = ex.Message });
        }
    }

    [HttpGet("PresupuestoCuentas")]
    public async Task<IActionResult> PresupuestoCuentas([FromQuery] string nombre,int anio)
    {
        try
        {
            var parameters = new ReportParameter[]
            {
                new ReportParameter("anio", anio.ToString())
            };

            var reportBytes = await _reportService.GenerateReportAsync($"/Reports_GestorPresupuestos/{nombre}", parameters);

            return File(reportBytes, "application/pdf", "PresupuestoGeneralReport.pdf");
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = ex.Message });
        }
    }

    [HttpGet("PresupuestoGeneralTotal")]
    public async Task<IActionResult> PresupuestoGeneralTotal([FromQuery] string fechainicio,string fechafin)
    {
        try
        {
            string anio = DateTime.Parse(fechainicio).Year.ToString();
            var parameters = new ReportParameter[]
            {
                new ReportParameter("anio", anio.ToString()),
                new ReportParameter("fechainicio",fechainicio),
                new ReportParameter("fechafin", fechafin)
            };

            var reportBytes = await _reportService.GenerateReportAsync($"/Reports_GestorPresupuestos/PresupuestoGeneralTotal", parameters);

            return File(reportBytes, "application/pdf", "PresupuestoGeneralReport.pdf");
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = ex.Message });
        }
    }

    [HttpGet("PresupuestoGeneralTotalExcel")]
    public async Task<IActionResult> PresupuestoCuentaslExcel([FromQuery] string fechainicio, string fechafin)
    {
        try
        {
            string anio = DateTime.Parse(fechainicio).Year.ToString();
            var parameters = new ReportParameter[]
            {
                new ReportParameter("anio", anio.ToString()),
                new ReportParameter("fechainicio",fechainicio),
                new ReportParameter("fechafin", fechafin)
            };

            // Call the GenerateReportExcelAsync for Excel export
            var reportBytes = await _reportService.GenerateReportExcelAsync($"/Reports_GestorPresupuestos/PresupuestoGeneralTotal", parameters);

            return File(reportBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "PresupuestoGeneralReport.xlsx");
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = ex.Message });
        }
    }


}
