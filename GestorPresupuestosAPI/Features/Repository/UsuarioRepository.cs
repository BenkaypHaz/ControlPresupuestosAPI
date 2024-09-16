using GestorPresupuestosAPI.Infraestructure.DataBases;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

public class UsuarioRepository
{
    private readonly GestorPresupuestosAHM _context;

    public UsuarioRepository(GestorPresupuestosAHM context)
    {
        _context = context;
    }

    public async Task<List<Usuarios>> GetAllUsuariosAsync()
    {
        return await _context.usuarios.ToListAsync();
    }

    public async Task<Usuarios> GetUserByIdAsync(int id)
    {
        return await _context.usuarios.FirstOrDefaultAsync(u => u.IdUsuario == id);
    }


    public async Task<Usuarios> AddUsuarioAsync(Usuarios user)
    {
        string salt;
        user.ClaveEncriptada = Security.HashPassword(user.ClaveEncriptada, out salt);
        _context.usuarios.Add(user);
        await _context.SaveChangesAsync();
        return user;
    }

    public async Task UpdateUserAsync(int id,Usuarios userToUpdate)
    {
        var existingUser = await _context.usuarios.FirstOrDefaultAsync(u => u.IdUsuario == id);
        if (existingUser != null)
        {
            existingUser.Usuario = userToUpdate.Usuario;
            existingUser.Nombre = userToUpdate.Nombre;
            existingUser.Correo = userToUpdate.Correo;
            existingUser.IdDepartamento = userToUpdate.IdDepartamento;
            existingUser.IdRol = userToUpdate.IdRol;

            await _context.SaveChangesAsync();
        }
    }


    public async Task DeleteUserAsync(int id)
    {
        var user = await _context.usuarios.FirstOrDefaultAsync(u => u.IdUsuario == id);
        if (user != null)
        {
            user.activo = false;
            await _context.SaveChangesAsync();
        }
    }

    public async Task<Usuarios> GetUserByUsernameAsync(string username)
    {
        return await _context.usuarios.FirstOrDefaultAsync(u => u.Usuario == username);
    }


    public async Task<RolesUsuario> GetUserRoleAsync(int userId)
    {
        return await _context.RolesUsuario.FirstOrDefaultAsync(ru => ru.UsuId == userId);
    }

    public async Task<List<Roles> >GetRoles()
    {
        return await _context.Roles.ToListAsync();
    }
    public async Task<List<string>> GetPermissionsByRoleAsync(int roleId)
    {
        return await _context.Permisos
                             .Where(p => p.RoleId == roleId)
                             .Select(p => p.Permiso)
                             .ToListAsync();
    }


    public async Task UpdatePasswordAsync(int userId, string newPasswordHash)
    {
        var user = await _context.usuarios.FindAsync(userId);
        if (user != null)
        {
            user.ClaveEncriptada = newPasswordHash;
            await _context.SaveChangesAsync();
        }
    }


}
