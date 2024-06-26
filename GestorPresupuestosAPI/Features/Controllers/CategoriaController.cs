using GestorPresupuestosAPI.Features.Utility;
using GestorPresupuestosAPI.Infraestructure.Modelos;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

[Route("api/[controller]")]
[ApiController]
public class CategoriasController : ControllerBase
{
    private readonly CategoriaService _categoriaService;

    public CategoriasController(CategoriaService categoriaService)
    {
        _categoriaService = categoriaService;
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse>> GetAllCategorias()
    {
        var response = await _categoriaService.GetAllCategoriasAsync();
        return Ok(response);
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse>> AddCategoria([FromBody] Categorias categoria)
    {
        if (categoria == null)
        {
            return BadRequest(ApiResponse.BadRequest("Invalid categoria data"));
        }

        var response = await _categoriaService.AddCategoriaAsync(categoria);
        if (!response.Success)
        {
            return BadRequest(response);
        }

        return CreatedAtAction(nameof(GetAllCategorias), new { id = categoria.IdCat }, response);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse>> GetCategoriaById(int id)
    {
        var response = await _categoriaService.GetCategoriaByIdAsync(id);
        if (!response.Success)
        {
            return NotFound(response);
        }
        return Ok(response);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ApiResponse>> UpdateCategoria(int id, [FromBody] Categorias categoria)
    {
        if (categoria == null || id != categoria.IdCat)
        {
            return BadRequest(ApiResponse.BadRequest("Invalid request"));
        }

        var response = await _categoriaService.UpdateCategoriaAsync(categoria);
        if (!response.Success)
        {
            return BadRequest(response);
        }
        return Ok(response);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<ApiResponse>> DeleteCategoria(int id)
    {
        var response = await _categoriaService.DeleteCategoriaAsync(id);
        if (!response.Success)
        {
            return NotFound(response);
        }
        return Ok(response);
    }
}
