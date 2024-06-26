using GestorPresupuestosAPI.Infraestructure.DataBases;
using GestorPresupuestosAPI.Infraestructure.Modelos;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

public class PresupuestoCuentaRepository
{
    private readonly GestorPresupuestosAHM _context;

    public PresupuestoCuentaRepository(GestorPresupuestosAHM context)
    {
        _context = context;
    }

    public async Task<List<PresupuestoCuenta>> GetAllPresupuestoCuentasAsync()
    {
        return await _context.PresupuestoCuentas                           
                             .ToListAsync();
    }


    public async Task<List<CuentaDetailDTO>> GetAllCuentas()
    {
        var results = await (from pc in _context.PresupuestoCuentas
                             join cu in _context.cuenta on pc.IdCuenta equals cu.IdCuenta
                             select new
                             {
                                 cu.Descripcions,
                                 pc.Descripcion,
                                 pc.Cantidad,
                                 pc.Comentarios
                             })
                          .ToListAsync();

        return results
            .GroupBy(r => r.Descripcions) 
            .Select(group => new CuentaDetailDTO
            {
                DescripcionCuenta = group.Key,
                Details = group.Select(item => new PresupuestoCuentaDetail
                {
                    Descripcion = item.Descripcion,
                    Cantidad = item.Cantidad,
                    Comentarios = item.Comentarios
                }).ToList()
            }).ToList();
    }

    public async Task<List<CuentaDetailDTO>> GetAllCuentasById(int idPresu)
    {
        var results = await (from pc in _context.PresupuestoCuentas
                             join cu in _context.cuenta on pc.IdCuenta equals cu.IdCuenta
                             where pc.IdPresu == idPresu 
                             select new
                             {
                                 cu.Descripcions,
                                 pc.Descripcion,
                                 pc.Cantidad,
                                 pc.Comentarios,
                                 pc.IdCuentas,
                                 pc.Ejecutada,
                                 pc.EjecucionParcial
                             })
                            .ToListAsync();

        return results
            .GroupBy(r => r.Descripcions)
            .Select(group => new CuentaDetailDTO
            {
                DescripcionCuenta = group.Key,
                Details = group.Select(item => new PresupuestoCuentaDetail
                {
                    Descripcion = item.Descripcion,
                    Cantidad = item.Cantidad,
                    Comentarios = item.Comentarios,
                    IdCuenta = item.IdCuentas,
                    Ejecutada = item.Ejecutada,
                    EjecutadaParcial = item.EjecucionParcial
                }).ToList()
            }).ToList();
    }

    public async Task<PresupuestoSummaryDTO> GetPresupuestoSummaryAsync()
    {
        var lastPresupuestoCantidad = await _context.presupuesto.OrderByDescending(p => p.IdPresu).FirstOrDefaultAsync();

        var NoEjecutadas = await _context.PresupuestoCuentas.CountAsync(pc => !pc.Ejecutada);
        var Ejecutadas = await _context.PresupuestoCuentas.CountAsync(pc => pc.Ejecutada);

        var sumEjecutadas = await _context.PresupuestoCuentas.Where(pc => pc.Ejecutada).SumAsync(pc => pc.Cantidad);
        var disponible = lastPresupuestoCantidad.Cantidad - sumEjecutadas;

        return new PresupuestoSummaryDTO
        {
            Ejecutadas = Ejecutadas,
            NoEjecutadas = NoEjecutadas,
            Presupuesto = lastPresupuestoCantidad.Cantidad,
            Disponible = disponible,
            Gastado = sumEjecutadas
        };
    }

    public async Task<PresupuestoSummaryDTO> GetPresupuestoSummaryById(int idpresu)
    {
        decimal lastPresupuestoCantidad = await _context.presupuesto
         .Where(x => x.IdPresu == idpresu)
         .Select(x => x.Cantidad)
         .SingleAsync();

        var NoEjecutadas = await _context.PresupuestoCuentas.Where(x => x.IdPresu == idpresu).CountAsync(pc => !pc.Ejecutada);
        var Ejecutadas = await _context.PresupuestoCuentas.Where(x => x.IdPresu == idpresu).CountAsync(pc => pc.Ejecutada);

        List<int> idCuentas = await _context.PresupuestoCuentas
            .Where(x => x.IdPresu == idpresu) 
            .Select(x => x.IdCuenta)
            .ToListAsync();

        var sumsParciales = await _context.ItemsEjecucionParcial.Where(x => x.Activo)
          .Join(_context.PresupuestoCuentas.Where(pc => pc.IdPresu == idpresu),
           iep => iep.id_cuenta,
           pc => pc.IdCuentas,
           (iep, pc) => iep.cantidad)
           .SumAsync(cantidad => cantidad);

        var sumsEjecuciones = await _context.ItemsEjecutados.Where(x => x.Activo)
        .Join(_context.PresupuestoCuentas.Where(pc => pc.IdPresu == idpresu),
        iep => iep.id_cuenta,
        pc => pc.IdCuentas,
        (iep, pc) => iep.cantidad_final)
        .SumAsync(cantidad => cantidad);

        Decimal Total = sumsParciales + sumsEjecuciones;

        var disponible = lastPresupuestoCantidad - Total;

        return new PresupuestoSummaryDTO
        {
            Ejecutadas = Ejecutadas,
            NoEjecutadas = NoEjecutadas,
            Presupuesto = lastPresupuestoCantidad,
            Disponible = disponible,
            Gastado = Total
        };
    }



    public async Task<PresupuestoCuenta> GetPresupuestoCuentaByIdAsync(int id)
    {
        return await _context.PresupuestoCuentas                           
                             .FirstOrDefaultAsync(pc => pc.IdCuentas == id);
    }
    public async Task AddPresupuestoCuentasAsync(List<PresupuestoCuenta> cuentas)
    {
        _context.PresupuestoCuentas.AddRange(cuentas);
        await _context.SaveChangesAsync();
    }
    public async Task<PresupuestoCuenta> AddPresupuestoCuentaAsync(PresupuestoCuenta presupuestoCuenta)
    {
        _context.PresupuestoCuentas.Add(presupuestoCuenta);
        await _context.SaveChangesAsync();
        return presupuestoCuenta;
    }

    public async Task UpdatePresupuestoCuentaAsync(PresupuestoCuenta presupuestoCuentaToUpdate)
    {
        var existingPresupuestoCuenta = await _context.PresupuestoCuentas.FirstOrDefaultAsync(pc => pc.IdCuentas == presupuestoCuentaToUpdate.IdCuentas);
        if (existingPresupuestoCuenta != null)
        {
            existingPresupuestoCuenta.IdPresu = presupuestoCuentaToUpdate.IdPresu;
            existingPresupuestoCuenta.IdCuenta = presupuestoCuentaToUpdate.IdCuenta;
            existingPresupuestoCuenta.Descripcion = presupuestoCuentaToUpdate.Descripcion;
            existingPresupuestoCuenta.Cantidad = presupuestoCuentaToUpdate.Cantidad;
            existingPresupuestoCuenta.Comentarios = presupuestoCuentaToUpdate.Comentarios;
            await _context.SaveChangesAsync();
        }
    }

    public async Task DeletePresupuestoCuentaAsync(int id)
    {
        var presupuestoCuenta = await _context.PresupuestoCuentas.FirstOrDefaultAsync(pc => pc.IdCuentas == id);
        if (presupuestoCuenta != null)
        {
            _context.PresupuestoCuentas.Remove(presupuestoCuenta);
            await _context.SaveChangesAsync();
        }
    }

    public async Task DesactivarEjeParcial(int id)
    {
        var presupuestoCuenta = await _context.ItemsEjecucionParcial.Where(x => x.id_ejecupar == id).FirstOrDefaultAsync();
        if (presupuestoCuenta != null)
        {
            presupuestoCuenta.Activo = false;
            await _context.SaveChangesAsync();
        }
    }

    public async Task DesactivarEjecucion(int id)
    {
        var presupuestoCuenta = await _context.ItemsEjecutados
            .Where(x => x.id_cuenta == id)
            .OrderByDescending(x => x.id_ejecu)  
            .FirstOrDefaultAsync();

        if (presupuestoCuenta != null)
        {
            var cuenta = await _context.PresupuestoCuentas.Where(x => x.IdCuentas == presupuestoCuenta.id_cuenta).FirstOrDefaultAsync();
            if(cuenta != null)
            {
                cuenta.Ejecutada = false;
                cuenta.EjecucionParcial = false;
            }
            presupuestoCuenta.Activo = false;
            await _context.SaveChangesAsync();
        }
    }

    public async Task<List<PresupuestoCuenta>> FindByPresupuestoIdAsync(int idPresu)
    {
        return await _context.PresupuestoCuentas
                             .Where(pc => pc.IdPresu == idPresu)
                             .ToListAsync();
    }

    public async Task DeletePresupuestoCuentasAsync(List<PresupuestoCuenta> cuentas)
    {
        _context.PresupuestoCuentas.RemoveRange(cuentas);
        await _context.SaveChangesAsync();
    }
    public async Task AddItemsEjecutadosAsync(ItemsEjecutados item)
    {
        using (var transaction = await _context.Database.BeginTransactionAsync())
        {
            try
            {
                _context.ItemsEjecutados.Add(item);
                await _context.SaveChangesAsync();

                var presupuestoCuenta = await _context.PresupuestoCuentas
                                                      .FirstOrDefaultAsync(pc => pc.IdCuentas == item.id_cuenta);

                if (presupuestoCuenta != null)
                {
                    presupuestoCuenta.Ejecutada = true;
                    await _context.SaveChangesAsync();
                }

                await transaction.CommitAsync();
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }

    public async Task AddItemsEjecucionParcialAsync(ItemsEjecucionParcial item)
    {
        using (var transaction = await _context.Database.BeginTransactionAsync())
        {
            try
            {
                _context.ItemsEjecucionParcial.Add(item);
                await _context.SaveChangesAsync();

                var presupuestoCuenta = await _context.PresupuestoCuentas
                                                      .FirstOrDefaultAsync(pc => pc.IdCuentas == item.id_cuenta);

                if (presupuestoCuenta != null)
                {
                    presupuestoCuenta.EjecucionParcial = true;
                    await _context.SaveChangesAsync();
                }

                await transaction.CommitAsync();
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }

    public async Task<PresupuestoCuentaInfoDTO> FindItemCuentaResumed(int idcuentas)
    {
        using (var transaction = await _context.Database.BeginTransactionAsync())
        {
            try
            {
                PresupuestoCuentaInfoDTO PresuCuentas = new PresupuestoCuentaInfoDTO
                {
                    EjecucionesParciales = new List<ListadoEjecParciales>()
                };
                PresupuestoCuenta cuenta = await _context.PresupuestoCuentas
                                                         .Where(x => x.IdCuentas == idcuentas)
                                                         .FirstOrDefaultAsync();

                if (cuenta != null)
                {
                    decimal totalSumado = 0;

                    if (cuenta.EjecucionParcial)
                    {
                        var ejecucionesParciales = await _context.ItemsEjecucionParcial
                                                                 .Where(x => x.id_cuenta == idcuentas && x.Activo)
                                                                 .ToListAsync();

                        foreach (var ejecParcial in ejecucionesParciales)
                        {
                            var proveedor = await _context.proveedores
                                                          .Where(p => p.id_prov == ejecParcial.proveedor)
                                                          .Select(p => p.descripcion)
                                                          .FirstOrDefaultAsync();

                            var ejecParcialDTO = new ListadoEjecParciales
                            {
                                id_ejecupar = ejecParcial.id_ejecupar,
                                id_cuenta = ejecParcial.id_cuenta,
                                cantidad = ejecParcial.cantidad,
                                fecha_compra = ejecParcial.fecha_compra,
                                observaciones = ejecParcial.observaciones,
                                proveedor = ejecParcial.proveedor,
                                proveedorName = proveedor
                            };

                            PresuCuentas.EjecucionesParciales.Add(ejecParcialDTO);
                        }

                        PresuCuentas.fecha_compra = ejecucionesParciales.Select(x => x.fecha_compra).FirstOrDefault();
                        totalSumado = ejecucionesParciales.Sum(x => x.cantidad);



                    }
                    else
                    {
                        var itemEjecutado = await _context.ItemsEjecutados
                                                          .Where(x => x.id_cuenta == idcuentas)
                                                          .FirstOrDefaultAsync();
                        if (itemEjecutado != null)
                        {
                            totalSumado = itemEjecutado.cantidad_final;
                            PresuCuentas.fecha_compra = itemEjecutado.fecha_compra;
                        }

                    }

                    PresuCuentas.IdCuentas = cuenta.IdCuentas;
                    PresuCuentas.IdPresu = cuenta.IdPresu;
                    PresuCuentas.IdCuenta = cuenta.IdCuenta;
                    PresuCuentas.Descripcion = cuenta.Descripcion;
                    PresuCuentas.Cantidad = cuenta.Cantidad;
                    PresuCuentas.Comentarios = cuenta.Comentarios;
                    PresuCuentas.Ejecutada = cuenta.Ejecutada;
                    PresuCuentas.EjecucionParcial = cuenta.EjecucionParcial;
                    PresuCuentas.TotalSumado = totalSumado;

                    await transaction.CommitAsync();
                    return PresuCuentas;
                }

                await transaction.RollbackAsync();
                return null;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new Exception($"An error occurred while processing the request: {ex.Message}", ex);
            }
        }
    }



}
