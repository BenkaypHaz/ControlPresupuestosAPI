using GestorPresupuestosAPI.Features.Utility;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

[Route("api/[controller]")]
[ApiController]
public class VersionesController : ControllerBase
{
    private readonly VersionesService _versionesService;

    public VersionesController(VersionesService versionesService)
    {
        _versionesService = versionesService;
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse>> GetAllVersiones()
    {
        var response = await _versionesService.GetAllVersionesAsync();
        return Ok(response);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse>> GetVersionById(int id)
    {
        var response = await _versionesService.GetVersionByIdAsync(id);
        if (!response.Success)
        {
            return NotFound(response);
        }
        return Ok(response);
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse>> AddVersion(Versiones versionDto)
    {
        var response = await _versionesService.AddVersionAsync(versionDto);
        if (!response.Success)
        {
            return BadRequest(response);
        }
        return Ok(response);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ApiResponse>> UpdateVersion(int id, Versiones versionDto)
    {
        versionDto.Id = id;
        var response = await _versionesService.UpdateVersionAsync(versionDto);
        if (!response.Success)
        {
            return BadRequest(response);
        }
        return Ok(response);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<ApiResponse>> DeleteVersion(int id)
    {
        var response = await _versionesService.DeleteVersionAsync(id);
        if (!response.Success)
        {
            return BadRequest(response);
        }
        return Ok(response);
    }
}
