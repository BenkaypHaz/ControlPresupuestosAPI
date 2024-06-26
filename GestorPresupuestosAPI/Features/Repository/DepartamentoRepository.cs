using GestorPresupuestosAPI.Infraestructure.DataBases;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

public class DepartamentoRepository
{
    private readonly GestorPresupuestosAHM _context;

    public DepartamentoRepository(GestorPresupuestosAHM context)
    {
        _context = context;
    }

    public async Task<List<Departamentos>> GetAllDepartamentosAsync()
    {
        return await _context.departamentos.ToListAsync();
    }

    public async Task<Departamentos> GetDepartamentoByIdAsync(int id)
    {
        return await _context.departamentos.FirstOrDefaultAsync(d => d.IdDepartamento == id);
    }

    public async Task<Departamentos> AddDepartamentoAsync(Departamentos departamento)
    {
        _context.departamentos.Add(departamento);
        await _context.SaveChangesAsync();
        return departamento;
    }

    public async Task UpdateDepartamentoAsync(Departamentos departamentoToUpdate)
    {
        var existingDepartamento = await _context.departamentos.FirstOrDefaultAsync(d => d.IdDepartamento == departamentoToUpdate.IdDepartamento);
        if (existingDepartamento != null)
        {
            existingDepartamento.Nombre = departamentoToUpdate.Nombre;
            await _context.SaveChangesAsync();
        }
    }

    public async Task DeleteDepartamentoAsync(int id)
    {
        var departamento = await _context.departamentos.FirstOrDefaultAsync(d => d.IdDepartamento == id);
        if (departamento != null)
        {
            _context.departamentos.Remove(departamento);
            await _context.SaveChangesAsync();
        }
    }
}
