using System.Collections.Generic;
using System.Threading.Tasks;
using GestorPresupuestosAPI.Infraestructure.DataBases;
using Microsoft.EntityFrameworkCore;

public class VersionesRepository
{
    private readonly GestorPresupuestosAHM _context;

    public VersionesRepository(GestorPresupuestosAHM context)
    {
        _context = context;
    }

    public async Task<Versiones> GetAllVersionesAsync()
    {
        return await _context.Versiones
                        .OrderByDescending(v => v.Id)
                        .FirstOrDefaultAsync();
    }

    public async Task<Versiones> GetVersionByIdAsync(int id)
    {
        return await _context.Versiones.FirstOrDefaultAsync(v => v.Id == id);
    }

    public async Task<Versiones> AddVersionAsync(Versiones version)
    {
        _context.Versiones.Add(version);
        await _context.SaveChangesAsync();
        return version;
    }

    public async Task UpdateVersionAsync(Versiones version)
    {
        _context.Versiones.Update(version);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteVersionAsync(int id)
    {
        var version = await GetVersionByIdAsync(id);
        if (version != null)
        {
            _context.Versiones.Remove(version);
            await _context.SaveChangesAsync();
        }
    }
}
