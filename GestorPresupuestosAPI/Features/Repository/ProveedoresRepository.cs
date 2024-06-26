using GestorPresupuestosAPI.Infraestructure.DataBases;
using GestorPresupuestosAPI.Infraestructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace GestorPresupuestosAPI.Features.Repository
{
    public class ProveedoresRepository
    {
        private readonly GestorPresupuestosAHM _context;

        public ProveedoresRepository(GestorPresupuestosAHM context)
        {
            _context = context;
        }

        public async Task<List<Proveedores>> GetAllProveedoresAsync()
        {
            return await _context.proveedores.ToListAsync();
        }

        public async Task<Cuenta> GetCuentaByIdAsync(int id)
        {
            return await _context.cuenta.FirstOrDefaultAsync(c => c.IdCuenta == id);
        }

        public async Task<Cuenta> AddCuentaAsync(Cuenta cuenta)
        {
            _context.cuenta.Add(cuenta);
            await _context.SaveChangesAsync();
            return cuenta;
        }

        public async Task UpdateCuentaAsync(Cuenta cuentaToUpdate)
        {
            var existingCuenta = await _context.cuenta.FirstOrDefaultAsync(c => c.IdCuenta == cuentaToUpdate.IdCuenta);
            if (existingCuenta != null)
            {
                existingCuenta.Descripcions = cuentaToUpdate.Descripcions;
                existingCuenta.IdDepartamento = cuentaToUpdate.IdDepartamento;
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteCuentaAsync(int id)
        {
            var cuenta = await _context.cuenta.FirstOrDefaultAsync(c => c.IdCuenta == id);
            if (cuenta != null)
            {
                _context.cuenta.Remove(cuenta);
                await _context.SaveChangesAsync();
            }
        }
    }
}
