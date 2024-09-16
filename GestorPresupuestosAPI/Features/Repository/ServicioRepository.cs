using System.Collections.Generic;
using System.Threading.Tasks;
using GestorPresupuestosAPI.Infraestructure.DataBases;
using GestorPresupuestosAPI.Infraestructure.Entities;
using Microsoft.EntityFrameworkCore;

public class ServicioRepository
{
    private readonly GestorPresupuestosAHM _context;

    public ServicioRepository(GestorPresupuestosAHM context)
    {
        _context = context;
    }

    public async Task<List<Servicio>> GetAllServiciosAsync()
    {
        return await _context.Servicios.ToListAsync();
    }

    public async Task<Servicio> GetServicioByIdAsync(int id)
    {
        return await _context.Servicios.FirstOrDefaultAsync(s => s.IdServi == id);
    }

    public async Task<Servicio> AddServicioAsync(Servicio servicio)
    {
        _context.Servicios.Add(servicio);
        await _context.SaveChangesAsync();
        return servicio;
    }

    public async Task<Servicio> UpdateServicioAsync(int id,Servicio servicio)
    {
        var existingServicio = await _context.Servicios.FirstOrDefaultAsync(s => s.IdServi == id);
        if (existingServicio != null)
        {
            existingServicio.Nombre = servicio.Nombre;
            await _context.SaveChangesAsync();
            return existingServicio;
        }
        return null;
    }

    public async Task<bool> DeleteServicioAsync(int id)
    {
        var servicio = await _context.Servicios.FirstOrDefaultAsync(s => s.IdServi == id);
        if (servicio != null)
        {
            _context.Servicios.Remove(servicio);
            await _context.SaveChangesAsync();
            return true;
        }
        return false;
    }
}
