using GestorPresupuestosAPI.Features.Services;
using GestorPresupuestosAPI.Features.Utility;
using Microsoft.AspNetCore.Mvc;

namespace GestorPresupuestosAPI.Features.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProveedoresController: ControllerBase
    {
        private readonly ProveedoresService _proveedoresService;

        public ProveedoresController(ProveedoresService proveedoresService)
        {
            _proveedoresService = proveedoresService;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse>> GetAllCuentas()
        {
            var response = await _proveedoresService.GetAllProveedoresAsync();
            return Ok(response);
        }
    }
}
