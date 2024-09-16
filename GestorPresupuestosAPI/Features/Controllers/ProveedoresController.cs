using Azure;
using GestorPresupuestosAPI.Features.Services;
using GestorPresupuestosAPI.Features.Utility;
using GestorPresupuestosAPI.Infraestructure.Entities;
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

        [HttpPost]
        public async Task<ActionResult<ApiResponse>> AddProveedor([FromBody] Proveedores proveedor)
        {
            var newProveedor = await _proveedoresService.AddProveedorAsync(proveedor);
            return Ok(newProveedor);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse>> UpdateProveedor(int id, [FromBody] Proveedores proveedor)
        {
            if (id != proveedor.id_prov)
            {
                return BadRequest(ApiResponse.BadRequest("ID mismatch."));
            }

            var response = await _proveedoresService.UpdateProveedorAsync(proveedor);
            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse>> DeleteProveedor(int id)
        {
            var response = await _proveedoresService.DeleteProveedorAsync(id);
            if (!response.Success)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }
    }
}
