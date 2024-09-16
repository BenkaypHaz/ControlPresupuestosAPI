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

        public async Task<Proveedores> AddAsync(Proveedores proveedor)
        {
            _context.proveedores.Add(proveedor);
            await _context.SaveChangesAsync();
            return proveedor;
        }

        public async Task UpdateAsync(Proveedores proveedor)
        {
            _context.proveedores.Update(proveedor);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var proveedor = await _context.proveedores.FindAsync(id);
            if (proveedor != null)
            {
                _context.proveedores.Remove(proveedor);
                await _context.SaveChangesAsync();
            }
        }
    }
}
