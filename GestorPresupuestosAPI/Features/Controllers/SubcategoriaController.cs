using GestorPresupuestosAPI.Features.Utility;
using GestorPresupuestosAPI.Infraestructure.Modelos;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

[Route("api/[controller]")]
[ApiController]
public class SubcategoriasController : ControllerBase
{
    private readonly SubcategoriaService _subcategoriaService;

    public SubcategoriasController(SubcategoriaService subcategoriaService)
    {
        _subcategoriaService = subcategoriaService;
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse>> GetAllSubcategorias()
    {
        var response = await _subcategoriaService.GetAllSubcategoriasAsync();
        return Ok(response);
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse>> AddSubcategoria([FromBody] Subcategorias subcategoria)
    {
        if (subcategoria == null)
        {
            return BadRequest(ApiResponse.BadRequest("Invalid subcategoria data"));
        }

        var response = await _subcategoriaService.AddSubcategoriaAsync(subcategoria);
        if (!response.Success)
        {
            return BadRequest(response);
        }

        return CreatedAtAction(nameof(GetAllSubcategorias), new { id = subcategoria.IdSubcate }, response);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse>> GetSubcategoriaById(int id)
    {
        var response = await _subcategoriaService.GetSubcategoriaByIdAsync(id);
        if (!response.Success)
        {
            return NotFound(response);
        }
        return Ok(response);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ApiResponse>> UpdateSubcategoria(int id, [FromBody] Subcategorias subcategoria)
    {
        if (subcategoria == null || id != subcategoria.IdSubcate)
        {
            return BadRequest(ApiResponse.BadRequest("Invalid request"));
        }

        var response = await _subcategoriaService.UpdateSubcategoriaAsync(subcategoria);
        if (!response.Success)
        {
            return BadRequest(response);
        }
        return Ok(response);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<ApiResponse>> DeleteSubcategoria(int id)
    {
        var response = await _subcategoriaService.DeleteSubcategoriaAsync(id);
        if (!response.Success)
        {
            return NotFound(response);
        }
        return Ok(response);
    }
}
