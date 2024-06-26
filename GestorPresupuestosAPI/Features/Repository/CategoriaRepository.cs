using GestorPresupuestosAPI.Infraestructure.DataBases;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

public class CategoriaRepository
{
    private readonly GestorPresupuestosAHM _context;

    public CategoriaRepository(GestorPresupuestosAHM context)
    {
        _context = context;
    }

    public async Task<List<Categorias>> GetAllCategoriasAsync()
    {
        return await _context.categorias.Where(c => c.Activo).ToListAsync();
    }
    public async Task<Categorias> GetCategoriaByIdAsync(int id)
    {
        return await _context.categorias.FirstOrDefaultAsync(c => c.IdCat == id && c.Activo);
    }

    public async Task<Categorias> AddCategoriaAsync(Categorias categoria)
    {
        _context.categorias.Add(categoria);
        await _context.SaveChangesAsync();
        return categoria;
    }

    public async Task UpdateCategoriaAsync(Categorias categoriaToUpdate)
    {
        var existingCategoria = await _context.categorias.FirstOrDefaultAsync(c => c.IdCat == categoriaToUpdate.IdCat);
        if (existingCategoria != null)
        {
            existingCategoria.Nombre = categoriaToUpdate.Nombre;
            await _context.SaveChangesAsync();
        }
    }

    public async Task DeactivateCategoriaAsync(int id)
    {
        var categoria = await _context.categorias.FirstOrDefaultAsync(c => c.IdCat == id);
        if (categoria != null)
        {
            categoria.Activo = false;
            await _context.SaveChangesAsync();
        }
    }
}
