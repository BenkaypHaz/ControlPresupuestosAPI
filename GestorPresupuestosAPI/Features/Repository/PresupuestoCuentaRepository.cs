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
                                 pc.EjecucionParcial,
                                 pc.Modificada
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
                    EjecutadaParcial = item.EjecucionParcial,
                    Modificada = item.Modificada
                }).ToList()
            }).ToList();
    }
    public async Task<PresupuestoSummaryDTO> GetPresupuestoSummaryByDepartment(int idDepartamento)
    {
        decimal cantidadTotal = await _context.presupuesto
            .Where(p => p.id_departamento == idDepartamento)
            .SumAsync(x => x.Cantidad);

        var NoEjecutadas = await _context.PresupuestoCuentas
            .Join(_context.presupuesto,
                  pc => pc.IdPresu,
                  p => p.IdPresu,
                  (pc, p) => new { pc, p })
            .Where(joined => !joined.pc.Ejecutada && joined.p.id_departamento == idDepartamento)
            .CountAsync();

        var Ejecutadas = await _context.PresupuestoCuentas
            .Join(_context.presupuesto,
                  pc => pc.IdPresu,
                  p => p.IdPresu,
                  (pc, p) => new { pc, p })
            .Where(joined => joined.pc.Ejecutada && joined.p.id_departamento == idDepartamento)
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
        decimal lastPresupuestoCantidad = await _context.PresupuestoCuentas
         .Where(x => x.IdPresu == idpresu)
         .SumAsync(x => x.Cantidad);

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

    public async Task<PresupuestoSummaryDTO> GetSummaryPresuEgreso()
    {
        decimal cantidadTotal = await _context.presupuesto
         .SumAsync(x => x.Cantidad);
       

        var NoEjecutadas = await _context.PresupuestoCuentas.CountAsync(pc => !pc.Ejecutada);
        var Ejecutadas = await _context.PresupuestoCuentas.CountAsync(pc => pc.Ejecutada);

        List<int> idCuentas = await _context.PresupuestoCuentas
            .Select(x => x.IdCuenta)
            .ToListAsync();

        var sumsParciales = await _context.ItemsEjecucionParcial.Where(x => x.Activo)
          .Join(_context.PresupuestoCuentas,
           iep => iep.id_cuenta,
           pc => pc.IdCuentas,
           (iep, pc) => iep.cantidad)
           .SumAsync(cantidad => cantidad);

        var sumsEjecuciones = await _context.ItemsEjecutados.Where(x => x.Activo)
        .Join(_context.PresupuestoCuentas,
        iep => iep.id_cuenta,
        pc => pc.IdCuentas,
        (iep, pc) => iep.cantidad_final)
        .SumAsync(cantidad => cantidad);

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


    public async Task<PresupuestoSummaryDTO> GetSummaryAllPresuDepto(int depto)
    {
        decimal cantidadTotal = await _context.presupuesto
        .Where(x=>x.id_departamento == depto)
         .SumAsync(x => x.Cantidad);


        var NoEjecutadas = await _context.PresupuestoCuentas.CountAsync(pc => !pc.Ejecutada);
        var Ejecutadas = await _context.PresupuestoCuentas.CountAsync(pc => pc.Ejecutada);

        List<int> idCuentas = await _context.PresupuestoCuentas
            .Select(x => x.IdCuenta)
            .ToListAsync();

        var sumsParciales = await _context.ItemsEjecucionParcial.Where(x => x.Activo)
          .Join(_context.PresupuestoCuentas,
           iep => iep.id_cuenta,
           pc => pc.IdCuentas,
           (iep, pc) => iep.cantidad)
           .SumAsync(cantidad => cantidad);

        var sumsEjecuciones = await _context.ItemsEjecutados.Where(x => x.Activo)
        .Join(_context.PresupuestoCuentas,
        iep => iep.id_cuenta,
        pc => pc.IdCuentas,
        (iep, pc) => iep.cantidad_final)
        .SumAsync(cantidad => cantidad);

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
    public async Task AddItemsEjecutadoAdminsAsync(ItemsEjecutados item, int usumodifica, string comentario)
    {
        using (var transaction = await _context.Database.BeginTransactionAsync())
        {
            try
            {
                _context.ItemsEjecutados.Add(item);
                await _context.SaveChangesAsync();

                var cuenta = await _context.PresupuestoCuentas.FirstOrDefaultAsync(pc => pc.IdCuentas == item.id_cuenta);
                if (cuenta == null)
                {
                    throw new Exception("PresupuestoCuenta not found.");
                }

                var presu = await _context.presupuesto.FirstOrDefaultAsync(p => p.IdPresu == cuenta.IdPresu);
                if (presu == null)
                {
                    throw new Exception("Presupuesto not found.");
                }

                var usuario = await _context.usuarios.FirstOrDefaultAsync(u => u.IdUsuario == presu.usu_crea);
                if (usuario == null)
                {
                    throw new Exception("Usuario not found.");
                }

                var usuarioModifica = await _context.usuarios.FirstOrDefaultAsync(u => u.IdUsuario == usumodifica);
                if (usuarioModifica == null)
                {
                    throw new Exception("Usuario Modifica not found.");
                }

                DateTime fechaHoy = DateTime.Today;

                var noti = new Notificaciones
                {
                    PresupuestoId = presu.IdPresu,
                    CuentaId = cuenta.IdCuentas,
                    Modificacion = $"El usuario {usuarioModifica.Nombre} ha ejecutado {cuenta.Descripcion} del presupuesto {presu.nombre} el {fechaHoy.ToShortDateString()}.",
                    UsuModifica = usuarioModifica.IdUsuario,
                    UsuPresupuesto = usuario.IdUsuario,
                    Comentario = comentario,
                    Leida = false
                };

                _context.Notificaciones.Add(noti);

                cuenta.Ejecutada = true;
                cuenta.Modificada = true;

                await _context.SaveChangesAsync();

                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new Exception($"An error occurred while adding the executed items: {ex.Message}", ex);
            }
        }
    }

    public async Task AddItemsEjecucionParcialAdminAsync(ItemsEjecucionParcial item, int usumodifica, string comentario)
    {
        using (var transaction = await _context.Database.BeginTransactionAsync())
        {
            try
            {
                _context.ItemsEjecucionParcial.Add(item);
                await _context.SaveChangesAsync();

                var cuenta = await _context.PresupuestoCuentas.FirstOrDefaultAsync(pc => pc.IdCuentas == item.id_cuenta);
                if (cuenta == null)
                {
                    throw new Exception("PresupuestoCuenta not found.");
                }

                var presu = await _context.presupuesto.FirstOrDefaultAsync(p => p.IdPresu == cuenta.IdPresu);
                if (presu == null)
                {
                    throw new Exception("Presupuesto not found.");
                }

                var usuario = await _context.usuarios.FirstOrDefaultAsync(u => u.IdUsuario == presu.usu_crea);
                if (usuario == null)
                {
                    throw new Exception("Usuario not found.");
                }

                var usuarioModifica = await _context.usuarios.FirstOrDefaultAsync(u => u.IdUsuario == usumodifica);
                if (usuarioModifica == null)
                {
                    throw new Exception("Usuario Modifica not found.");
                }

                DateTime fechaHoy = DateTime.Today;

                var noti = new Notificaciones
                {
                    PresupuestoId = presu.IdPresu,
                    CuentaId = cuenta.IdCuentas,
                    Modificacion = $"El usuario {usuarioModifica.Nombre} ha realizado una ejecucion parcial {cuenta.Descripcion} del presupuesto {presu.nombre} el {fechaHoy.ToShortDateString()}.",
                    UsuModifica = usuarioModifica.IdUsuario,
                    UsuPresupuesto = usuario.IdUsuario,
                    Comentario = comentario,
                    Leida = false
                };

                _context.Notificaciones.Add(noti);

                cuenta.EjecucionParcial = true;
                cuenta.Modificada = true;

                await _context.SaveChangesAsync();

                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new Exception($"An error occurred while adding the partial execution items: {ex.Message}", ex);
            }
        }
    }

    public async Task UpdatePresupuestoCuentaAsync(PresupuestoCuentaUpdateDTO dto)
    {
        string nombreAnterior = "";
        var cuenta = await _context.PresupuestoCuentas
                                   .FirstOrDefaultAsync(pc => pc.IdCuentas == dto.IdCuentas);
        if (cuenta == null)
        {
            throw new Exception("PresupuestoCuenta not found.");
        }

        var newCuenta = await _context.cuenta.FirstOrDefaultAsync(c => c.IdCuenta == dto.IdCuenta);
        var oldCuenta = await _context.cuenta.FirstOrDefaultAsync(c => c.IdCuenta == cuenta.IdCuenta);

        var presu = await _context.presupuesto.FirstOrDefaultAsync(p => p.IdPresu == cuenta.IdPresu);

        var usuarioModifica = await _context.usuarios.FirstOrDefaultAsync(u => u.IdUsuario == dto.UsuModifica);

        if (presu == null || usuarioModifica == null)
        {
            throw new Exception("Required entity not found.");
        }

        var usuario = await _context.usuarios.FirstOrDefaultAsync(u => u.IdUsuario == presu.usu_crea);
        if (usuario == null)
        {
            throw new Exception("Usuario not found.");
        }
        DateTime fechaHoy = DateTime.Today;
        List<string> modifications = new List<string>();

        if (cuenta.IdCuenta != dto.IdCuenta && newCuenta != null)
        {
            modifications.Add($"Cuenta asignada anteriormente: {oldCuenta.Descripcions} - Cuenta actual: {newCuenta.Descripcions}");
            cuenta.IdCuenta = dto.IdCuenta; 
        }
        if (cuenta.Descripcion != dto.Descripcion)
        {
            modifications.Add($"Nombre anterior: {cuenta.Descripcion} - Nombre actual: {dto.Descripcion}");
            nombreAnterior = cuenta.Descripcion;
            cuenta.Descripcion = dto.Descripcion;
        }
        if (cuenta.Cantidad != dto.Cantidad)
        {
            modifications.Add($"Cantidad anterior: {cuenta.Cantidad} - Cantidad actual: {dto.Cantidad}");
            cuenta.Cantidad = dto.Cantidad;
        }

        cuenta.Modificada = dto.Modificada;

        foreach (var modification in modifications)
        {
            var noti = new Notificaciones
            {
                PresupuestoId = presu.IdPresu,
                CuentaId = cuenta.IdCuentas,
                Modificacion = $"El usuario {usuarioModifica.Nombre} ha modificado el item {nombreAnterior} del presupuesto {presu.nombre}.  {modification}. Fecha modificacion: {fechaHoy.ToShortDateString()}.",
                UsuModifica = usuarioModifica.IdUsuario,
                UsuPresupuesto = usuario.IdUsuario,
                Comentario = "Prueba",
                Leida = false
            };
            _context.Notificaciones.Add(noti);
        }

        await _context.SaveChangesAsync();
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
                            // Determine where to fetch the "proveedor" name
                            string proveedorName;
                            if (ejecParcial.EsIngreso)
                            {
                                // Fetch from Servicios if EsIngreso is true
                                proveedorName = await _context.Servicios
                                                             .Where(s => s.IdServi == ejecParcial.proveedor)
                                                             .Select(s => s.Nombre)
                                                             .FirstOrDefaultAsync();
                            }
                            else
                            {
                                // Fetch from Proveedores if EsIngreso is false
                                proveedorName = await _context.proveedores
                                                              .Where(p => p.id_prov == ejecParcial.proveedor)
                                                              .Select(p => p.descripcion)
                                                              .FirstOrDefaultAsync();
                            }

                            var ejecParcialDTO = new ListadoEjecParciales
                            {
                                id_ejecupar = ejecParcial.id_ejecupar,
                                id_cuenta = ejecParcial.id_cuenta,
                                cantidad = ejecParcial.cantidad,
                                fecha_compra = ejecParcial.fecha_compra,
                                observaciones = ejecParcial.observaciones,
                                proveedor = ejecParcial.proveedor,
                                proveedorName = proveedorName // Now the provider name can come from either table
                            };

                            PresuCuentas.EjecucionesParciales.Add(ejecParcialDTO);
                        }

                        PresuCuentas.fecha_compra = ejecucionesParciales.Select(x => x.fecha_compra).FirstOrDefault();
                        totalSumado = ejecucionesParciales.Sum(x => x.cantidad);

                    }
                    else
                    {
                        var itemEjecutado = await _context.ItemsEjecutados
                                                          .Where(x => x.id_cuenta == idcuentas && x.Activo)
                                                          .FirstOrDefaultAsync();
                        if (itemEjecutado != null)
                        {
                            totalSumado = itemEjecutado.cantidad_final;
                            PresuCuentas.fecha_compra = itemEjecutado.fecha_compra;
                        }
                    }

                    // Set the remaining properties of PresuCuentas
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
