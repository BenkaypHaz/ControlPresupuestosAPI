using GestorPresupuestosAPI.Infraestructure.DataBases;
using GestorPresupuestosAPI.Infraestructure.Entities;
using GestorPresupuestosAPI.Infraestructure.Modelos;
using Microsoft.EntityFrameworkCore;

namespace GestorPresupuestosAPI.Features.Repository
{
    public class CuentasxSubCuentasRepository
    {
        private readonly GestorPresupuestosAHM _context;

        public CuentasxSubCuentasRepository(GestorPresupuestosAHM context)
        {
            _context = context;
        }

        public async Task<List<CuentaxSubCuentasDTO>> GetAllCuentasxSubCuentasAsync()
        {
            var result = await (from cxsc in _context.CuentasxSubCuentas
                                join c1 in _context.cuenta on cxsc.IdCuenta equals c1.IdCuenta
                                join c2 in _context.cuenta on cxsc.IdSubCuenta equals c2.IdCuenta
                                select new CuentaxSubCuentasDTO
                                {
                                    Id = cxsc.Id,
                                    IdCuenta = cxsc.IdCuenta,
                                    IdSubCuenta = cxsc.IdSubCuenta,
                                    Cuenta = c1.Descripcions,  
                                    SubCuenta = c2.Descripcions
                                }).ToListAsync();

            return result;
        }

        public async Task<List<Cuentas_Madre>> GetCuentasMadre()
        {
            return await _context.CuentasMadre.ToListAsync();
        }
        public async Task<List<Cuentas_Generales>> GetCuentasTitulo()
        {
            return await _context.CuentasTitulo.ToListAsync();
        }
        public async Task<CuentasxSubCuentas> GetCuentasxSubCuentasByIdAsync(int id)
        {
            return await _context.CuentasxSubCuentas.FirstOrDefaultAsync(x => x.Id == id);
        }
        public async Task<List<CuentaxSubCuentasDTO>> GetSubCuentasByIdCuenta(int id)
        {
            var result = await (from cxsc in _context.CuentasxSubCuentas
                                join cm1 in _context.CuentasMadre on cxsc.IdCuenta equals cm1.IdCuenta
                                join cm2 in _context.cuenta on cxsc.IdSubCuenta equals cm2.IdCuenta
                                where cxsc.IdCuenta == id
                                select new CuentaxSubCuentasDTO
                                {
                                    Id = cxsc.Id,
                                    IdCuenta = cxsc.IdCuenta,
                                    IdSubCuenta = cxsc.IdSubCuenta,
                                    Cuenta = cm1.Descripcions, 
                                    SubCuenta = cm2.Descripcions 
                                }).ToListAsync();

            return result;
        }
        public async Task<List<CuentaxSubCuentasDTO>> GetCuentasMadreById(int id)
        {
            var result = await (from cxsc in _context.GeneralesxMadre
                                join cm1 in _context.CuentasTitulo on cxsc.IdGeneral equals cm1.IdCuenta
                                join cm2 in _context.CuentasMadre on cxsc.IdMadre equals cm2.IdCuenta
                                where cxsc.IdGeneral == id
                                select new CuentaxSubCuentasDTO
                                {
                                    Id = cxsc.Id,
                                    IdCuenta = cxsc.IdGeneral,
                                    IdSubCuenta = cxsc.IdMadre,
                                    Cuenta = cm1.nombre,
                                    SubCuenta = cm2.Descripcions
                                }).ToListAsync();

            return result;
        }
        public async Task<Cuentas_Madre> AddCuentaMadre(Cuentas_Madre cuenta)
        {
            _context.CuentasMadre.Add(cuenta);
            await _context.SaveChangesAsync();
            return cuenta;
        }
        public async Task<Cuentas_Generales> AddCuentaTitulo(Cuentas_Generales cuenta)
        {
            _context.CuentasTitulo.Add(cuenta);
            await _context.SaveChangesAsync();
            return cuenta;
        }
        public async Task<CuentasxSubCuentas> AddCuentasxSubCuentasAsync(CuentasxSubCuentas cuentasxSubCuentas)
        {
            _context.CuentasxSubCuentas.Add(cuentasxSubCuentas);
            await _context.SaveChangesAsync();
            return cuentasxSubCuentas;
        }

        public async Task UpdateCuentasxSubCuentasAsync(CuentasxSubCuentas cuentasxSubCuentas)
        {
            _context.CuentasxSubCuentas.Update(cuentasxSubCuentas);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateCuentaMadre(Cuentas_Madre cuentaToUpdate)
        {
            var existingCuenta = await _context.CuentasMadre.FirstOrDefaultAsync(c => c.IdCuenta == cuentaToUpdate.IdCuenta);
            if (existingCuenta != null)
            {
                existingCuenta.Descripcions = cuentaToUpdate.Descripcions;
                await _context.SaveChangesAsync();
            }
        }
        public async Task UpdateCuentaTitulo(Cuentas_Generales cuentaToUpdate)
        {
            var existingCuenta = await _context.CuentasTitulo.FirstOrDefaultAsync(c => c.IdCuenta == cuentaToUpdate.IdCuenta);
            if (existingCuenta != null)
            {
                existingCuenta.nombre = cuentaToUpdate.nombre;
                await _context.SaveChangesAsync();
            }
        }
        public async Task DeleteCuentasxSubCuentasAsync(int id)
        {
            var cuentasxSubCuentas = await GetCuentasxSubCuentasByIdAsync(id);
            if (cuentasxSubCuentas != null)
            {
                _context.CuentasxSubCuentas.Remove(cuentasxSubCuentas);
                await _context.SaveChangesAsync();
            }
        }
        public async Task<GeneralesxMadre> GetGeneralesxMadre(int id)
        {
            return await _context.GeneralesxMadre.FirstOrDefaultAsync(x => x.Id == id);
        }
        public async Task BorrarRelacion(int id)
        {
            var cuentasxSubCuentas = await GetGeneralesxMadre(id);
            if (cuentasxSubCuentas != null)
            {
                _context.GeneralesxMadre.Remove(cuentasxSubCuentas);
                await _context.SaveChangesAsync();
            }
        }

    }
}
