using GestorPresupuestosAPI.Features.Utility;
using GestorPresupuestosAPI.Infraestructure.Modelos;
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


    public async Task<ApiResponse> UpdateUserAsync(Usuarios user)
    {
        try
        {
            var existingUser = await _usuarioRepository.GetUserByIdAsync(user.IdUsuario);
            if (existingUser == null)
            {
                return ApiResponse.NotFound($"User with ID {user.IdUsuario} not found.");
            }

            await _usuarioRepository.UpdateUserAsync(user);
            return ApiResponse.Ok($"User with ID {user.IdUsuario} updated successfully.");
        }
        catch (Exception ex)
        {
            return ApiResponse.BadRequest($"An error occurred while updating the user: {ex.Message}");
        }
    }


    public async Task<ApiResponse> DeleteUserAsync(int id)
    {
        try
        {
            var user = await _usuarioRepository.GetUserByIdAsync(id);
            if (user == null)
            {
                return ApiResponse.NotFound($"User with ID {id} not found.");
            }

            await _usuarioRepository.DeleteUserAsync(id);
            return ApiResponse.Ok($"User with ID {id} deleted successfully.");
        }
        catch (Exception ex)
        {
            return ApiResponse.BadRequest($"An error occurred while 'deleting' the user: {ex.Message}");
        }
    }

    public async Task<bool> AuthenticateUserAsync(Credentials credentials)
    {
        var user = await _usuarioRepository.GetUserByUsernameAsync(credentials.Username);
        if (user == null) return false;

        if (user.ClaveEncriptada.Contains(":"))
        {
            var parts = user.ClaveEncriptada.Split(':');
            var salt = parts[0];
            var storedHash = parts[1];
            var inputHash = Security.HashPasswordWithSalt(credentials.Password, salt);
            return inputHash == storedHash;
        }

        return false;
    }

}
