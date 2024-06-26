using GestorPresupuestosAPI.Infraestructure.DataBases;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

public class SubcategoriaRepository
{
    private readonly GestorPresupuestosAHM _context;

    public SubcategoriaRepository(GestorPresupuestosAHM context)
    {
        _context = context;
    }

    public async Task<List<Subcategorias>> GetAllSubcategoriasAsync()
    {
        return await _context.subcategorias.ToListAsync();
    }

    public async Task<Subcategorias> GetSubcategoriaByIdAsync(int id)
    {
        return await _context.subcategorias.FirstOrDefaultAsync(s => s.IdSubcate == id);
    }

    public async Task<Subcategorias> AddSubcategoriaAsync(Subcategorias subcategoria)
    {
        _context.subcategorias.Add(subcategoria);
        await _context.SaveChangesAsync();
        return subcategoria;
    }

    public async Task UpdateSubcategoriaAsync(Subcategorias subcategoriaToUpdate)
    {
        var existingSubcategoria = await _context.subcategorias.FirstOrDefaultAsync(s => s.IdSubcate == subcategoriaToUpdate.IdSubcate);
        if (existingSubcategoria != null)
        {
            existingSubcategoria.Nombre = subcategoriaToUpdate.Nombre;
            existingSubcategoria.CategoriaId = subcategoriaToUpdate.CategoriaId;
            await _context.SaveChangesAsync();
        }
    }

    public async Task DeleteSubcategoriaAsync(int id)
    {
        var subcategoria = await _context.subcategorias.FirstOrDefaultAsync(s => s.IdSubcate == id);
        if (subcategoria != null)
        {
            _context.subcategorias.Remove(subcategoria);
            await _context.SaveChangesAsync();
        }
    }
}
