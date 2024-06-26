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

    public async Task UpdateUserAsync(Usuarios userToUpdate)
    {
        var existingUser = await _context.usuarios.FirstOrDefaultAsync(u => u.IdUsuario == userToUpdate.IdUsuario);
        if (existingUser != null)
        {
            existingUser.Usuario = userToUpdate.Usuario;
            existingUser.Nombre = userToUpdate.Nombre;
            existingUser.ClaveEncriptada = userToUpdate.ClaveEncriptada; 
            existingUser.Correo = userToUpdate.Correo;
            existingUser.activo = userToUpdate.activo;
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

}
