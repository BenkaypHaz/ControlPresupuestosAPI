using GestorPresupuestosAPI.Features.Utility;
using GestorPresupuestosAPI.Infraestructure.Modelos;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public class UsuarioService
{
    private readonly UsuarioRepository _usuarioRepository;

    public UsuarioService(UsuarioRepository usuarioRepository)
    {
        _usuarioRepository = usuarioRepository;
    }

    public async Task<ApiResponse> GetAllUsuariosAsync()
    {
        try
        {
            var usuarios = await _usuarioRepository.GetAllUsuariosAsync();
            return ApiResponse.Ok("Usuarios retrieved successfully", usuarios);
        }
        catch (Exception ex)
        {
            return ApiResponse.BadRequest($"An error occurred: {ex.Message}");
        }
    }


    public async Task<ApiResponse> AddUsuarioAsync(Usuarios user)
    {
        try
        {
            var createdUser = await _usuarioRepository.AddUsuarioAsync(user);
            return ApiResponse.Ok("Usuario added successfully", createdUser);
        }
        catch (Exception ex)
        {
            return ApiResponse.BadRequest($"An error occurred while adding the user: {ex.Message}");
        }
    }


    public async Task<ApiResponse> GetUserByIdAsync(int id)
    {
        try
        {
            var user = await _usuarioRepository.GetUserByIdAsync(id);
            if (user == null)
            {
                return ApiResponse.NotFound($"User with ID {id} not found.");
            }
            return ApiResponse.Ok("User found", user);
        }
        catch (Exception ex)
        {
            return ApiResponse.BadRequest($"An error occurred: {ex.Message}");
        }
    }
    public async Task<ApiResponse> GetRoles()
    {
        try
        {
            var roles = await _usuarioRepository.GetRoles();
            if (roles == null)
            {
                return ApiResponse.NotFound($"No se encontraron los roles.");
            }
            return ApiResponse.Ok("Roles encontrados", roles);
        }
        catch (Exception ex)
        {
            return ApiResponse.BadRequest($"An error occurred: {ex.Message}");
        }
    }

    public async Task<ApiResponse> UpdateUserAsync(int id,Usuarios user)
    {
        try
        {
            var existingUser = await _usuarioRepository.GetUserByIdAsync(id);
            if (existingUser == null)
            {
                return ApiResponse.NotFound($"User with ID {user.IdUsuario} not found.");
            }

            await _usuarioRepository.UpdateUserAsync(id,user);
            return ApiResponse.Ok($"User with ID {id} updated successfully.");
        }
        catch (Exception ex)
        {
            return ApiResponse.BadRequest($"An error occurred while updating the user: {ex.Message}");
        }
    }


    public async Task<ApiResponse> ChangePasswordAsync(int userId, string newPassword)
    {
        try
        {
            var user = await _usuarioRepository.GetUserByIdAsync(userId);
            if (user == null)
            {
                return ApiResponse.NotFound("User not found.");
            }

            string salt;
            string newPasswordHash = Security.HashPassword(newPassword, out salt);

            await _usuarioRepository.UpdatePasswordAsync(userId, newPasswordHash);

            return ApiResponse.Ok("Password changed successfully.");
        }
        catch (Exception ex)
        {
            return ApiResponse.BadRequest($"An error occurred while changing the password: {ex.Message}");
        }
    }

    public async Task<ApiResponse> AuthenticateUserAsync(Credentials credentials)
        {
            var user = await _usuarioRepository.GetUserByUsernameAsync(credentials.Username);
            if (user == null) return ApiResponse.BadRequest("User not found.");

            if (user.ClaveEncriptada.Contains(":"))
            {
                var parts = user.ClaveEncriptada.Split(':');
                var salt = parts[0];
                var storedHash = parts[1];
                var inputHash = Security.HashPasswordWithSalt(credentials.Password, salt);
                if (inputHash != storedHash) return ApiResponse.Unauthorized("Invalid password.");
            }
            else
            {
                return ApiResponse.Unauthorized("Invalid password format.");
            }
     
                var permisos = await _usuarioRepository.GetPermissionsByRoleAsync((int)user.IdRol);

                var data = new
                {
                    User = user,
                    Permisos = permisos
                };

                return ApiResponse.Ok("User authenticated successfully.", data);            

        }
    


}
