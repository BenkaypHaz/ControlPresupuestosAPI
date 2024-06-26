using GestorPresupuestosAPI.Infraestructure.DataBases;
using GestorPresupuestosAPI.Infraestructure.Entities;
using GestorPresupuestosAPI.Infraestructure.Modelos;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class PresupuestoRepository
{
    private readonly GestorPresupuestosAHM _context;

    public PresupuestoRepository(GestorPresupuestosAHM context)
    {
        _context = context;
    }

    public async Task<List<Presupuesto>> GetAllPresupuestosAsync()
    {
        return await _context.presupuesto.Where(p => p.Activo).ToListAsync();
    }

    public async Task<List<Tipo_presupuesto>> GetTipoPresupuesto()
    {
        return await _context.Tipo_presupuesto.ToListAsync();
    }

    public async Task<Presupuesto> GetPresupuestoByIdAsync(int id)
    {
        return await _context.presupuesto.FirstOrDefaultAsync(p => p.IdPresu == id && p.Activo);
    }

    public async Task<Presupuesto> AddPresupuestoAsync(decimal cantidad, int idpresu)
    {
        var presupuesto = await _context.presupuesto.FindAsync(idpresu);
        if (presupuesto != null)
        {
            presupuesto.Cantidad = cantidad;

            await _context.SaveChangesAsync();
        }
        return presupuesto; 
    }

    public async Task<PresupuestoWithCuentasDTO> GetPresupuestoInfo(int id)
    {
        decimal cantidad = await _context.presupuesto
                                          .Where(x => x.IdPresu == id)
                                          .Select(x => x.Cantidad)
                                          .FirstOrDefaultAsync();

        List<PresupuestoCuenta> cuentas = await _context.PresupuestoCuentas
                                                        .Where(x => x.IdPresu == id)
                                                        .ToListAsync();

        PresupuestoWithCuentasDTO presu = new PresupuestoWithCuentasDTO
        {
            cantidad = cantidad,
            Cuentas = cuentas
        };

        return presu;
    }


    public async Task<Presupuesto> CrearPresupuesto(string nombre, int tipo,int usuId)
    {
        Presupuesto presu = new Presupuesto();
        presu.nombre = nombre;
        presu.Cantidad = 0;
        presu.tipo_presupuesto=tipo;
        presu.usu_crea = usuId; 
        presu.Activo = true;
        presu.FechaCreacion = DateTime.Today;
        presu.estado = 1;
        _context.presupuesto.Add(presu);
        await _context.SaveChangesAsync();
        return presu;
    }

    public async Task UpdatePresupuestoAsync(Presupuesto presupuestoToUpdate)
    {
        var existingPresupuesto = await _context.presupuesto.FirstOrDefaultAsync(p => p.IdPresu == presupuestoToUpdate.IdPresu);
        if (existingPresupuesto != null)
        {
            existingPresupuesto.Cantidad = presupuestoToUpdate.Cantidad;
            existingPresupuesto.FechaCreacion = presupuestoToUpdate.FechaCreacion;
            existingPresupuesto.Activo = presupuestoToUpdate.Activo;
            await _context.SaveChangesAsync();
        }
    }

    public async Task DeactivatePresupuestoAsync(int id)
    {
        var presupuesto = await _context.presupuesto.FirstOrDefaultAsync(p => p.IdPresu == id);
        if (presupuesto != null)
        {
            presupuesto.Activo = false;  // Set Activo to false instead of deleting
            await _context.SaveChangesAsync();
        }
    }
}
