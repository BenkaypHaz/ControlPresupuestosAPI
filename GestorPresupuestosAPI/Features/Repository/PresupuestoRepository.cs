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

    public async Task<List<PresupuestoDepa>> GetAllPresupuestosAsync()
    {
        var presupuestos = await (from p in _context.presupuesto
                                  join d in _context.departamentos on p.id_departamento equals d.IdDepartamento
                                  where p.Activo
                                  select new PresupuestoDepa
                                  {
                                      IdPresu = p.IdPresu,
                                      nombre = p.nombre,
                                      Cantidad = p.Cantidad,
                                      FechaCreacion = p.FechaCreacion,
                                      estado = p.estado,
                                      Activo = p.Activo,
                                      usu_crea = p.usu_crea,
                                      Departamento = d.Nombre
                                  }).ToListAsync();

        return presupuestos;
    }

    public async Task<PresupuestoCardModel> GetPresupuestoById(int idpresu)
    {
        var presupuesto = await (from p in _context.presupuesto
                                 join d in _context.departamentos on p.id_departamento equals d.IdDepartamento
                                 where p.Activo && p.IdPresu == idpresu
                                 select new
                                 {
                                     p.nombre,
                                     p.Cantidad,
                                     EstadoCode = p.estado,
                                     p.FechaCreacion,
                                     DepartamentoNombre = d.Nombre
                                 }).FirstOrDefaultAsync();

        if (presupuesto == null)
            return null;

        var estadoDescription = presupuesto.EstadoCode switch
        {
            0 => "Creandose",
            1 => "Pendiente",
            2 => "Revisar",
            3 => "Aprobado",
            _ => "Estado Desconocido"
        };

        return new PresupuestoCardModel
        {
            Nombre = presupuesto.nombre,
            Cantidad = presupuesto.Cantidad,
            Estado = estadoDescription,
            Departamento = presupuesto.DepartamentoNombre
        };
    }

    public async Task<List<PresupuestoDepa>> GetAllPresupuestosAdminAsync()
    {
        var presupuestos = await (from p in _context.presupuesto
                                  join d in _context.departamentos on p.id_departamento equals d.IdDepartamento
                                  where p.Activo && (p.estado==1) 
                                  select new PresupuestoDepa
                                  {
                                      IdPresu = p.IdPresu,
                                      nombre = p.nombre,
                                      Cantidad = p.Cantidad,
                                      FechaCreacion = p.FechaCreacion,
                                      estado = p.estado,
                                      Activo = p.Activo,
                                      usu_crea = p.usu_crea,
                                      Departamento = d.Nombre
                                  }).ToListAsync();

        return presupuestos;
    }
    public async Task<List<PresupuestoDepa>> GetAllPresupuestosAprobadosAdminAsync()
    {
        var presupuestos = await (from p in _context.presupuesto
                                  join d in _context.departamentos on p.id_departamento equals d.IdDepartamento
                                  where p.Activo && (p.estado == 3)
                                  select new PresupuestoDepa
                                  {
                                      IdPresu = p.IdPresu,
                                      nombre = p.nombre,
                                      Cantidad = p.Cantidad,
                                      FechaCreacion = p.FechaCreacion,
                                      estado = p.estado,
                                      Activo = p.Activo,
                                      usu_crea = p.usu_crea,
                                      Departamento = d.Nombre
                                  }).ToListAsync();

        return presupuestos;
    }

    public async Task<List<Tipo_presupuesto>> GetTipoPresupuesto()
    {
        return await _context.Tipo_presupuesto.ToListAsync();
    }
    public async Task<List<PresupuestoDepa>> GetPresupuestoByUsuId(int id)
    {
        var presupuestos = await (from p in _context.presupuesto
                                  join d in _context.departamentos on p.id_departamento equals d.IdDepartamento
                                  where p.Activo && p.usu_crea == id 
                                  select new PresupuestoDepa
                                  {
                                      IdPresu = p.IdPresu,
                                      nombre = p.nombre,
                                      Cantidad = p.Cantidad,
                                      FechaCreacion = p.FechaCreacion,
                                      Activo = p.Activo,
                                      estado = p.estado,
                                      usu_crea = p.usu_crea,
                                      Departamento = d.Nombre
                                  }).ToListAsync();

        return presupuestos;
    }
    public async Task<List<PresupuestoDepa>> GetPresupuestoCreadoById(int id)
    {
        var presupuestos = await (from p in _context.presupuesto
                                  join d in _context.departamentos on p.id_departamento equals d.IdDepartamento
                                  where p.Activo && p.usu_crea == id && (p.estado == 0 || p.estado == 1 || p.estado==2)    
                                  select new PresupuestoDepa
                                  {
                                      IdPresu = p.IdPresu,
                                      nombre = p.nombre,
                                      Cantidad = p.Cantidad,
                                      FechaCreacion = p.FechaCreacion,
                                      Activo = p.Activo,
                                      estado = p.estado,
                                      usu_crea = p.usu_crea,
                                      Departamento = d.Nombre
                                  }).ToListAsync();

        return presupuestos;
    }
    public async Task<List<PresupuestoDepa>> GetPresupuestoByUsuAsync(int id)
    {
        var presupuestos = await (from p in _context.presupuesto
                                  join d in _context.departamentos on p.id_departamento equals d.IdDepartamento
                                  where p.Activo && p.usu_crea==id && p.estado == 3
                                  select new PresupuestoDepa
                                  {
                                      IdPresu = p.IdPresu,
                                      nombre = p.nombre,
                                      Cantidad = p.Cantidad,
                                      FechaCreacion = p.FechaCreacion,
                                      Activo = p.Activo,
                                      estado = p.estado,
                                      usu_crea = p.usu_crea,
                                      Departamento = d.Nombre
                                  }).ToListAsync();

        return presupuestos;
    }
    public async Task<List<PresupuestoDepa>> GetPresupuestoCreadoByUsuAsync(int id)
    {
        var presupuestos = await (from p in _context.presupuesto
                                  join d in _context.departamentos on p.id_departamento equals d.IdDepartamento
                                  where p.Activo && p.usu_crea == id && (p.estado == 1 || p.estado == 2)
                                  select new PresupuestoDepa
                                  {
                                      IdPresu = p.IdPresu,
                                      nombre = p.nombre,
                                      Cantidad = p.Cantidad,
                                      FechaCreacion = p.FechaCreacion,
                                      Activo = p.Activo,
                                      estado = p.estado,
                                      usu_crea = p.usu_crea,
                                      Departamento = d.Nombre
                                  }).ToListAsync();

        return presupuestos;
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
        Presupuesto presu = await _context.presupuesto
                                          .Where(x => x.IdPresu == id)
                                          .FirstOrDefaultAsync();

        List<PresupuestoCuenta> cuentas = await _context.PresupuestoCuentas
                                                        .Where(x => x.IdPresu == id)
                                                        .ToListAsync();

        PresupuestoWithCuentasDTO presuInfo = new PresupuestoWithCuentasDTO
        {
            cantidad = presu.Cantidad,
            Cuentas = cuentas,
            TipoPresu = presu.tipo_presupuesto
        };

        return presuInfo;
    }


    public async Task<Presupuesto> CrearPresupuesto(string nombre, int tipo,int usuId)
    {

        int idDepartamento = _context.usuarios.Where(x=>x.IdUsuario == usuId).Select(x => x.IdDepartamento).FirstOrDefault();

        Presupuesto presu = new Presupuesto();
        presu.nombre = nombre;
        presu.Cantidad = 0;
        presu.tipo_presupuesto=tipo;
        presu.usu_crea = usuId; 
        presu.Activo = true;
        presu.FechaCreacion = DateTime.Today;
        presu.estado = 0;
        presu.id_departamento = idDepartamento;
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
    public async Task UpdatePresupuestoEstado(int idPresu, int estado)
    {
        var existingPresupuesto = await _context.presupuesto.FirstOrDefaultAsync(p => p.IdPresu == idPresu);
        if (existingPresupuesto != null)
        {
            existingPresupuesto.estado = estado;
            await _context.SaveChangesAsync();
        }
    }
    public async Task UpdatePresupuestoEstadoAsync(int idPresu, int newEstado, int usumodifica)
    {
        using (var transaction = await _context.Database.BeginTransactionAsync())
        {
            try
            {
                var presu = await _context.presupuesto.FirstOrDefaultAsync(p => p.IdPresu == idPresu);
                if (presu == null)
                {
                    throw new Exception("Presupuesto not found.");
                }

                var oldEstado = presu.estado;
                presu.estado = newEstado;
                await _context.SaveChangesAsync();

                var usuarioModifica = await _context.usuarios.FirstOrDefaultAsync(u => u.IdUsuario == usumodifica);
                if (usuarioModifica == null)
                {
                    throw new Exception("Usuario Modifica not found.");
                }

                DateTime fechaHoy = DateTime.Today;

                var noti = new Notificaciones
                {
                    PresupuestoId = presu.IdPresu,
                    CuentaId = 0, 
                    Modificacion = $"El usuario {usuarioModifica.Nombre} ha modificado el estado del presupuesto {presu.nombre} de {estadoDescriptions[oldEstado]} a {estadoDescriptions[newEstado]} el {fechaHoy.ToShortDateString()}.",
                    UsuModifica = usuarioModifica.IdUsuario,
                    UsuPresupuesto = presu.usu_crea,
                    Comentario = "",
                    Leida = false
                };

                _context.Notificaciones.Add(noti);
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new Exception($"An error occurred while updating the estado of the presupuesto: {ex.Message}", ex);
            }
        }
    }

    public async Task DeactivatePresupuestoAsync(int id)
    {
        var presupuesto = await _context.presupuesto.FirstOrDefaultAsync(p => p.IdPresu == id);
        if (presupuesto != null)
        {
            presupuesto.Activo = false;  
            await _context.SaveChangesAsync();
        }
    }

    private readonly Dictionary<int, string> estadoDescriptions = new Dictionary<int, string>
    {
        { 0, "Creandose" },
        { 1, "En revisión" },
        { 2, "Corregir" },
        { 3, "Aprobado" }
    };

}
