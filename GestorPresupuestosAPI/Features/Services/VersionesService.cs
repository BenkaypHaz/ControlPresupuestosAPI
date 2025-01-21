using GestorPresupuestosAPI.Features.Utility;
using System.Collections.Generic;
using System.Threading.Tasks;

public class VersionesService
{
    private readonly VersionesRepository _versionesRepository;

    public VersionesService(VersionesRepository versionesRepository)
    {
        _versionesRepository = versionesRepository;
    }

    public async Task<ApiResponse> GetAllVersionesAsync()
    {
        try
        {
            var versiones = await _versionesRepository.GetAllVersionesAsync();
            return ApiResponse.Ok("Versiones retrieved successfully", versiones);
        }
        catch (Exception ex)
        {
            return ApiResponse.BadRequest($"An error occurred while retrieving versiones: {ex.Message}");
        }
    }

    public async Task<ApiResponse> GetVersionByIdAsync(int id)
    {
        try
        {
            var version = await _versionesRepository.GetVersionByIdAsync(id);
            if (version == null)
            {
                return ApiResponse.NotFound("Version not found");
            }
            return ApiResponse.Ok("Version retrieved successfully", version);
        }
        catch (Exception ex)
        {
            return ApiResponse.BadRequest($"An error occurred while retrieving the version: {ex.Message}");
        }
    }

    public async Task<ApiResponse> AddVersionAsync(Versiones versionDto)
    {
        try
        {
            var version = new Versiones
            {
                NumeroVersion = versionDto.NumeroVersion,
                Descripcion = versionDto.Descripcion,
                Fecha = versionDto.Fecha
            };
            var createdVersion = await _versionesRepository.AddVersionAsync(version);
            return ApiResponse.Ok("Version added successfully", createdVersion);
        }
        catch (Exception ex)
        {
            return ApiResponse.BadRequest($"An error occurred while adding the version: {ex.Message}");
        }
    }

    public async Task<ApiResponse> UpdateVersionAsync(Versiones versionDto)
    {
        try
        {
            var version = await _versionesRepository.GetVersionByIdAsync(versionDto.Id);
            if (version == null)
            {
                return ApiResponse.NotFound("Version not found");
            }

            version.NumeroVersion = versionDto.NumeroVersion;
            version.Descripcion = versionDto.Descripcion;
            version.Fecha = versionDto.Fecha;

            await _versionesRepository.UpdateVersionAsync(version);
            return ApiResponse.Ok("Version updated successfully", version);
        }
        catch (Exception ex)
        {
            return ApiResponse.BadRequest($"An error occurred while updating the version: {ex.Message}");
        }
    }

    public async Task<ApiResponse> DeleteVersionAsync(int id)
    {
        try
        {
            await _versionesRepository.DeleteVersionAsync(id);
            return ApiResponse.Ok("Version deleted successfully", null);
        }
        catch (Exception ex)
        {
            return ApiResponse.BadRequest($"An error occurred while deleting the version: {ex.Message}");
        }
    }
}
