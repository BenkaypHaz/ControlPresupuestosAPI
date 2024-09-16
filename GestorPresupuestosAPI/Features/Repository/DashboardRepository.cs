using GestorPresupuestosAPI.Infraestructure.DataBases;
using GestorPresupuestosAPI.Infraestructure.Modelos;
using Microsoft.EntityFrameworkCore;

namespace GestorPresupuestosAPI.Features.Repository
{
    public class DashboardRepository
    {
        private readonly GestorPresupuestosAHM _context;
        public DashboardRepository(GestorPresupuestosAHM context)
        {
            _context = context;
        }

        public async Task<PresupuestoSummaryDTO> GetSummaryPresupuestoByTipo(int tipoPresupuesto)
        {
            decimal cantidadTotal = await _context.presupuesto
                .Where(x => x.tipo_presupuesto == tipoPresupuesto)
                .SumAsync(x => x.Cantidad);

            var NoEjecutadas = await _context.PresupuestoCuentas
                .Join(_context.presupuesto,
                      pc => pc.IdPresu,
                      p => p.IdPresu,
                      (pc, p) => new { pc, p })
                .CountAsync(joined => !joined.pc.Ejecutada && joined.p.tipo_presupuesto == tipoPresupuesto);

            var Ejecutadas = await _context.PresupuestoCuentas
                .Join(_context.presupuesto,
                      pc => pc.IdPresu,
                      p => p.IdPresu,
                      (pc, p) => new { pc, p })
                .CountAsync(joined => joined.pc.Ejecutada && joined.p.tipo_presupuesto == tipoPresupuesto);

            List<int> idCuentas = await _context.PresupuestoCuentas
                .Join(_context.presupuesto,
                      pc => pc.IdPresu,
                      p => p.IdPresu,
                      (pc, p) => new { pc, p })
                .Where(joined => joined.p.tipo_presupuesto == tipoPresupuesto)
                .Select(joined => joined.pc.IdCuenta)
                .ToListAsync();

            var sumsParciales = await _context.ItemsEjecucionParcial
                .Where(x => x.Activo)
                .Join(_context.PresupuestoCuentas,
                      iep => iep.id_cuenta,
                      pc => pc.IdCuentas,
                      (iep, pc) => new { iep, pc })
                .Where(joined => idCuentas.Contains(joined.pc.IdCuentas))
                .SumAsync(joined => joined.iep.cantidad);

            var sumsEjecuciones = await _context.ItemsEjecutados
                .Where(x => x.Activo)
                .Join(_context.PresupuestoCuentas,
                      iep => iep.id_cuenta,
                      pc => pc.IdCuentas,
                      (iep, pc) => new { iep, pc })
                .Where(joined => idCuentas.Contains(joined.pc.IdCuentas))
                .SumAsync(joined => joined.iep.cantidad_final);

            decimal Total = sumsParciales + sumsEjecuciones;

            var disponible = cantidadTotal - Total;

            return new PresupuestoSummaryDTO
            {
                Ejecutadas = Ejecutadas,
                NoEjecutadas = NoEjecutadas,
                Presupuesto = cantidadTotal,
                Disponible = disponible,
                Gastado = Total
            };
        }
        public async Task<PresupuestoSummaryDTO> GetPresupuestoSummaryByDepartment(int idDepartamento, int tipoPresupuesto)
        {
            decimal cantidadTotal = await _context.presupuesto
                .Where(p => p.id_departamento == idDepartamento && p.tipo_presupuesto == tipoPresupuesto)
                .SumAsync(x => x.Cantidad);

            var NoEjecutadas = await _context.PresupuestoCuentas
                .Join(_context.presupuesto,
                      pc => pc.IdPresu,
                      p => p.IdPresu,
                      (pc, p) => new { pc, p })
                .Where(joined => !joined.pc.Ejecutada && joined.p.id_departamento == idDepartamento && joined.p.tipo_presupuesto == tipoPresupuesto)
                .CountAsync();

            var Ejecutadas = await _context.PresupuestoCuentas
                .Join(_context.presupuesto,
                      pc => pc.IdPresu,
                      p => p.IdPresu,
                      (pc, p) => new { pc, p })
                .Where(joined => joined.pc.Ejecutada && joined.p.id_departamento == idDepartamento && joined.p.tipo_presupuesto == tipoPresupuesto)
                .CountAsync();

            var sumsParciales = await _context.ItemsEjecucionParcial
                .Join(_context.PresupuestoCuentas,
                      iep => iep.id_cuenta,
                      pc => pc.IdCuentas,
                      (iep, pc) => new { iep, pc })
                .Join(_context.presupuesto,
                      joined => joined.pc.IdPresu,
                      p => p.IdPresu,
                      (joined, p) => new { joined.iep, p })
                .Where(joined => joined.p.id_departamento == idDepartamento)
                .SumAsync(x => x.iep.cantidad);

            var sumsEjecuciones = await _context.ItemsEjecutados
                .Join(_context.PresupuestoCuentas,
                      ie => ie.id_cuenta,
                      pc => pc.IdCuentas,
                      (ie, pc) => new { ie, pc })
                .Join(_context.presupuesto,
                      joined => joined.pc.IdPresu,
                      p => p.IdPresu,
                      (joined, p) => new { joined.ie, p })
                .Where(joined => joined.p.id_departamento == idDepartamento)
                .SumAsync(x => x.ie.cantidad_final);

            Decimal Total = sumsParciales + sumsEjecuciones;
            var disponible = cantidadTotal - Total;

            return new PresupuestoSummaryDTO
            {
                Ejecutadas = Ejecutadas,
                NoEjecutadas = NoEjecutadas,
                Presupuesto = cantidadTotal,
                Disponible = disponible,
                Gastado = Total
            };
        }

        public async Task<List<DatosPastel>> GetDepartamentoPresupuestoSummaryAsync()
        {
            var result = await (from presu in _context.presupuesto
                                join dep in _context.departamentos
                                on presu.id_departamento equals dep.IdDepartamento
                                where presu.tipo_presupuesto == 1
                                group presu by dep.Nombre into g
                                select new DatosPastel
                                {
                                    Nombre = g.Key,
                                    TotalCantidad = g.Sum(p => p.Cantidad)
                                })
                                .ToListAsync();

            return result;
        }

    }
}
