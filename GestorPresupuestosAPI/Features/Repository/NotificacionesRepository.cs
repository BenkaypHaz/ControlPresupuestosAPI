using GestorPresupuestosAPI.Infraestructure.DataBases;
using Microsoft.EntityFrameworkCore;

public class NotificacionesRepository
{
    private readonly GestorPresupuestosAHM _context;

    public NotificacionesRepository(GestorPresupuestosAHM context)
    {
        _context = context;
    }

    public async Task<List<Notificaciones>> GetAllNotificacionesAsync()
    {
        return await _context.Notificaciones.ToListAsync();
    }

    public async Task<List<Notificaciones>> GetNotificacionByIdAsync(int id)
    {
        return await _context.Notificaciones
            .Where(n => n.UsuPresupuesto == id)
            .OrderBy(n => n.IdNoti)
            .ToListAsync();
    }

    public async Task MarkNotificacionesAsLeidaAsync(List<int> ids)
    {
        foreach (var id in ids)
        {
            var notificacion = await _context.Notificaciones.FindAsync(id);
            if (notificacion != null)
            {
                notificacion.Leida = true;
            }
        }
        await _context.SaveChangesAsync();
    }
    public async Task<Notificaciones> AddNotificacionAsync(Notificaciones notificacion)
    {
        _context.Notificaciones.Add(notificacion);
        await _context.SaveChangesAsync();
        return notificacion;
    }

    public async Task UpdateNotificacionAsync(Notificaciones notificacion)
    {
        _context.Notificaciones.Update(notificacion);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteNotificacionAsync(int id)
    {
        var notificacion = await _context.Notificaciones.FirstOrDefaultAsync(n => n.IdNoti == id);
        if (notificacion != null)
        {
            _context.Notificaciones.Remove(notificacion);
            await _context.SaveChangesAsync();
        }
    }
}
