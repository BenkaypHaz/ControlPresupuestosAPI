using GestorPresupuestosAPI.Features.Utility;
using GestorPresupuestosAPI.Infraestructure.Modelos;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

[Route("api/[controller]")]
[ApiController]
public class UsuarioController : ControllerBase
{
    private readonly UsuarioService _usuarioService;

    public UsuarioController(UsuarioService usuarioService)
    {
        _usuarioService = usuarioService;
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse>> GetAllUsuarios()
    {
        var response = await _usuarioService.GetAllUsuariosAsync();
        return Ok(response);
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse>> AddUsuario([FromBody] Usuarios user)
    {
        if (user == null)
        {
            return BadRequest(ApiResponse.BadRequest("Invalid user data"));
        }

        var response = await _usuarioService.AddUsuarioAsync(user);
        if (!response.Success)
        {
            return BadRequest(response);
        }
       
        return CreatedAtAction(nameof(GetAllUsuarios), new { id = ((Usuarios)response.Data).IdUsuario }, response);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse>> GetUserById(int id)
    {
        var response = await _usuarioService.GetUserByIdAsync(id);
        if (!response.Success)
        {
            return NotFound(response);
        }
        return Ok(response);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ApiResponse>> UpdateUser(int id, [FromBody] Usuarios user)
    {
        if (user == null || id != user.IdUsuario)
        {
            return BadRequest(ApiResponse.BadRequest("Invalid request"));
        }

        var response = await _usuarioService.UpdateUserAsync(user);
        if (!response.Success)
        {
            return BadRequest(response);
        }
        return Ok(response); 
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<ApiResponse>> DeleteUser(int id)
    {
        var response = await _usuarioService.DeleteUserAsync(id);
        if (!response.Success)
        {
            return NotFound(response);
        }
        return Ok(response); 
    }

    [HttpPost("authenticate")]
    public async Task<ActionResult<ApiResponse>> Authenticate([FromBody] Credentials credentials)
    {
        if (!ModelState.IsValid)
            return BadRequest(ApiResponse.BadRequest("request Invalido"));

        bool isAuthenticated = await _usuarioService.AuthenticateUserAsync(credentials);

        if (!isAuthenticated)
            return Unauthorized(ApiResponse.BadRequest("Usuario o clave incorrectos"));

        return Ok(ApiResponse.Ok("Ingreso correcto"));
    }

}
