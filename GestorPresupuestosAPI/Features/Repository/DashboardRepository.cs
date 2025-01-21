using GestorPresupuestosAPI.Infraestructure.DataBases;
using GestorPresupuestosAPI.Infraestructure.Modelos;
using Microsoft.EntityFrameworkCore;
using System.Xml.Serialization;

namespace GestorPresupuestosAPI.Features.Repository
{
    public class DashboardRepository
    {
        private readonly GestorPresupuestosAHM _context;
        public DashboardRepository(GestorPresupuestosAHM context)
        {
            _context = context;
        }

        public async Task<PresupuestoSummaryDTO> GetSummaryPresupuestoByTipo(int tipoPresupuesto, int anio)
        {
            bool ingreso = tipoPresupuesto == 2;
            bool extra = false;
            List<int> idPresupuestos = await _context.presupuesto
                .Where(p => p.tipo_presupuesto == tipoPresupuesto && p.Activo && p.estado == 3 && p.Anio_Presupuesto==anio)
                .Select(p => p.IdPresu)
                .ToListAsync();

            var sumsParciales = await _context.ItemsEjecucionParcial
                .Join(_context.PresupuestoCuentas,
                      iep => iep.id_cuenta,
                      pc => pc.IdCuentas,
                      (iep, pc) => new { iep, pc })
                .Where(joined => idPresupuestos.Contains(joined.pc.IdPresu) && joined.iep.Activo && joined.iep.EsIngreso == ingreso && joined.pc.Activo==true)
                .SumAsync(joined => joined.iep.cantidad);

            var sumsEjecuciones = await _context.ItemsEjecutados
                .Join(_context.PresupuestoCuentas,
                      ie => ie.id_cuenta,
                      pc => pc.IdCuentas,
                      (ie, pc) => new { ie, pc })
                .Where(joined => idPresupuestos.Contains(joined.pc.IdPresu) && joined.ie.Activo && joined.ie.EsIngreso == ingreso && joined.pc.Activo == true)
                .SumAsync(joined => joined.ie.cantidad_final);

            decimal cantidadTotal = await _context.presupuesto
                .Where(p => idPresupuestos.Contains(p.IdPresu))
                .SumAsync(p => p.Cantidad);

            decimal totalGastado = sumsParciales + sumsEjecuciones;
            decimal disponible = cantidadTotal - totalGastado;


            if (tipoPresupuesto == 2 && disponible < 0)
            {
                disponible = Math.Abs(disponible);
                extra = true;
            }

            return new PresupuestoSummaryDTO
            {
                Presupuesto = cantidadTotal,
                Disponible = disponible,
                Gastado = totalGastado,
                ingresoExtra = extra
            };
        }



        public async Task<PresupuestoSummaryDTO> GetPresupuestoSummaryByDepartment(int idDepartamento, int tipoPresupuesto)
        {
            bool ingreso = tipoPresupuesto == 2;
            bool extra = false;

            var presupuestoQuery = _context.presupuesto
                .Where(p => p.id_departamento == idDepartamento && p.tipo_presupuesto == tipoPresupuesto && p.Activo && (p.estado == 3 || p.estado == 4));

            decimal cantidadTotal = await presupuestoQuery.SumAsync(p => p.Cantidad);

            List<int> idPresupuestos = await presupuestoQuery.Select(p => p.IdPresu).ToListAsync();

            var presupuestoCuentas = await _context.PresupuestoCuentas
                .Where(pc => idPresupuestos.Contains(pc.IdPresu) && pc.EsIngreso == ingreso && pc.Activo==true)
                .Select(pc => new
                {
                    pc.IdCuentas,
                    pc.Ejecutada,
                    pc.EjecucionParcial
                })
                .ToListAsync();

            var idCuentasEjecucion = presupuestoCuentas
                .Where(pc => pc.Ejecutada && !pc.EjecucionParcial)
                .Select(pc => pc.IdCuentas)
                .ToList();

            var idCuentasEjecucionParcial = presupuestoCuentas
                .Where(pc => pc.EjecucionParcial && !pc.Ejecutada)
                .Select(pc => pc.IdCuentas)
                .ToList();

            decimal sumEjecuciones = await _context.ItemsEjecutados
                .Where(ie => idCuentasEjecucion.Contains(ie.id_cuenta) && ie.Activo && ie.EsIngreso == ingreso)
                .SumAsync(ie => ie.cantidad_final);

            decimal sumParciales = await _context.ItemsEjecucionParcial
                .Where(iep => idCuentasEjecucionParcial.Contains(iep.id_cuenta) && iep.Activo && iep.EsIngreso == ingreso)
                .SumAsync(iep => iep.cantidad);

            decimal totalGastado = sumParciales + sumEjecuciones;
            decimal disponible = cantidadTotal - totalGastado;


            if (tipoPresupuesto == 2 && disponible < 0)
            {
                disponible = Math.Abs(disponible);
                extra = true;
            }

            return new PresupuestoSummaryDTO
            {
                Presupuesto = cantidadTotal,
                Disponible = disponible,
                Gastado = totalGastado,
                ingresoExtra = extra
            };
        }


        public async Task<List<DatosPastel>> GetDepartamentoPresupuestoSummaryAsync(int anio, int tipopresu)
        {
            var result = await (from presu in _context.presupuesto
                                join dep in _context.departamentos
                                on presu.id_departamento equals dep.IdDepartamento
                                where presu.tipo_presupuesto == tipopresu && presu.estado==3 && presu.Anio_Presupuesto==anio
                                group presu by dep.Nombre into g
                                select new DatosPastel
                                {
                                    Nombre = g.Key,
                                    TotalCantidad = g.Sum(p => p.Cantidad)
                                })
                                .ToListAsync();

            return result;
        }

        public async Task<StatPresupuestoIndividualDTO> StatsPresupuestoIndividual(int idPresu, int anio)
        {

            Presupuesto presupuesto = _context.presupuesto
     .Where(x => x.IdPresu == idPresu)
     .FirstOrDefault();

            var result = await (from presu in _context.presupuesto
                                join presucu in _context.PresupuestoCuentas on presu.IdPresu equals presucu.IdPresu
                                join itempar in _context.ItemsEjecucionParcial on presucu.IdCuentas equals itempar.id_cuenta into itemparGroup
                                from itempar in itemparGroup.DefaultIfEmpty()
                                join itemejecu in _context.ItemsEjecutados on presucu.IdCuentas equals itemejecu.id_cuenta into itemejecuGroup
                                from itemejecu in itemejecuGroup.DefaultIfEmpty() 
                                where presu.IdPresu == idPresu && presu.Anio_Presupuesto == anio
                                group new { presucu, itempar, itemejecu } by presu.IdPresu into g
                                select new StatPresupuestoIndividualDTO
                                {
                                    TotalEjecutado = presupuesto.tipo_presupuesto == 2
                                        ? g.Sum(x => (x.itempar != null && x.itempar.Activo ? x.itempar.cantidad : 0))
                                          + g.Sum(x => (x.itemejecu != null && x.itemejecu.Activo ? x.itemejecu.cantidad_final : 0))
                                        : g.Sum(x => (x.itempar != null && x.itempar.Activo ? x.itempar.cantidad : 0))
                                          + g.Sum(x => (x.itemejecu != null && x.itemejecu.Activo ? x.itemejecu.cantidad_final : 0)),

                                    TotalRestante = presupuesto.tipo_presupuesto == 2
                                        ? presupuesto.Cantidad - (
                                              g.Sum(x => (x.itempar != null && x.itempar.Activo ? x.itempar.cantidad : 0))
                                            + g.Sum(x => (x.itemejecu != null && x.itemejecu.Activo ? x.itemejecu.cantidad_final : 0))
                                          )
                                        : presupuesto.Cantidad - (
                                              g.Sum(x => (x.itempar != null && x.itempar.Activo ? x.itempar.cantidad : 0))
                                            + g.Sum(x => (x.itemejecu != null && x.itemejecu.Activo ? x.itemejecu.cantidad_final : 0))
                                          ),

                                    NoEjecutadas = g.Count(x => !x.presucu.EjecucionParcial && !x.presucu.Ejecutada),

                                    Ejecutadas = g.Count(x => x.presucu.EjecucionParcial || x.presucu.Ejecutada && (x.itemejecu.Activo == true || x.itempar.Activo == true))
                                })
                                .FirstOrDefaultAsync();

            return result ?? new StatPresupuestoIndividualDTO();

        }

        public async Task<List<StatsCuentasPresupuestado>> GetStatsCuentasPresupuestadoAsync(int presupuestoId, int anio)
        {
            var result = await (from presu in _context.presupuesto
                                join presucu in _context.PresupuestoCuentas on presu.IdPresu equals presucu.IdPresu
                                join cuenta in _context.cuenta on presucu.IdCuenta equals cuenta.IdCuenta
                                where presu.IdPresu == presupuestoId && presu.Anio_Presupuesto == anio

                                join ie in _context.ItemsEjecutados.Where(ie => ie.Activo == true) 
                                    on presucu.IdCuentas equals ie.id_cuenta into ejecutados
                                from ejec in ejecutados.DefaultIfEmpty()

                                join iep in _context.ItemsEjecucionParcial.Where(iep => iep.Activo == true)
                                    on presucu.IdCuentas equals iep.id_cuenta into ejecucionParcial
                                from par in ejecucionParcial.DefaultIfEmpty()

                                group new { presucu, ejec, par } by new { cuenta.IdCuenta, cuenta.Descripcions } into g
                                select new StatsCuentasPresupuestado
                                {
                                    Descripcion = g.Key.Descripcions,
                                    total_cantidad = g.Sum(x => x.presucu.Cantidad),
                                    total_ejecutado = g.Sum(x =>
                                        (x.ejec != null ? (decimal?)x.ejec.cantidad_final : 0) +
                                        (x.par != null ? (decimal?)x.par.cantidad : 0)
                                    ) ?? 0,
                                    total_restante = g.Sum(x => x.presucu.Cantidad) - g.Sum(x =>
                                        (x.ejec != null ? (decimal?)x.ejec.cantidad_final : 0) +
                                        (x.par != null ? (decimal?)x.par.cantidad : 0)
                                    ) ?? 0
                                }).ToListAsync();

            return result;
        }
        public async Task<List<StatsDepartamentoPresupuestado>> GetDepartamentoStatsPresupuestadoAsync(int year, int tipoPresupuesto)
        {
            bool esIngreso = tipoPresupuesto == 1 ? false : true;
            
            var ejecutadoCuentas = await (from presucu in _context.PresupuestoCuentas
                                          select new
                                          {
                                              IdCuentas = presucu.IdCuentas,
                                              Ejecutado = presucu.Ejecutada == true
                                                  ? _context.ItemsEjecutados
                                                      .Where(ie => ie.id_cuenta == presucu.IdCuentas && ie.Activo == true)
                                                      .Sum(ie => (decimal?)ie.cantidad_final) ?? 0
                                                  : presucu.EjecucionParcial == true
                                                  ? _context.ItemsEjecucionParcial
                                                      .Where(iep => iep.id_cuenta == presucu.IdCuentas && iep.Activo == true)
                                                      .Sum(iep => (decimal?)iep.cantidad) ?? 0
                                                  : 0
                                          }).ToDictionaryAsync(e => e.IdCuentas, e => e.Ejecutado);

            var presupuestoData = await (from presu in _context.presupuesto
                                         join presucu in _context.PresupuestoCuentas on presu.IdPresu equals presucu.IdPresu
                                         join dep in _context.departamentos on presu.id_departamento equals dep.IdDepartamento
                                         where presu.Anio_Presupuesto == year
                                               && presu.tipo_presupuesto == tipoPresupuesto
                                               && presucu.EsIngreso == esIngreso
                                               && presucu.Activo == true
                                         select new  
                                         {
                                             DepartamentoNombre = dep.Nombre,
                                             CantidadAsignada = presucu.Cantidad,
                                             Ejecutado = ejecutadoCuentas.ContainsKey(presucu.IdCuentas)
                                                         ? ejecutadoCuentas[presucu.IdCuentas] : 0
                                         }).ToListAsync();

            var result = presupuestoData
                .GroupBy(p => p.DepartamentoNombre)
                .Select(g => new StatsDepartamentoPresupuestado
                {
                    Departamento = g.Key,
                    TotalAsignado = g.Sum(x => x.CantidadAsignada),
                    TotalEjecutado = g.Sum(x => x.Ejecutado),
                    TotalDepto = presupuestoData
                        .Where(p => p.DepartamentoNombre == g.Key)
                        .Sum(p => p.CantidadAsignada)
                })
                .OrderBy(d => d.Departamento)
                .ToList();

            return result;
        }

        public async Task<List<StatsAnualesPresupuesto>> GetPresupuestoAnualStatsAsync(int tipo)
        {
            var result = await (from presu in _context.presupuesto
                                where presu.tipo_presupuesto == tipo
                                      && presu.estado == 3
                                group presu by presu.Anio_Presupuesto into g
                                select new StatsAnualesPresupuesto
                                {
                                    Anio = g.Key,
                                    cantidad = g.Sum(x => x.Cantidad)
                                })
                                .OrderBy(x => x.Anio)
                                .ToListAsync();

            return result;
        }


    }
}
