using GestorPresupuestosAPI.Infraestructure.DataBases;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using MimeKit;
using MailKit.Net.Smtp;
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

    public async Task<string> SendCodeToUserByEmailAsync(string email)
    {
        // Busca el usuario por correo
        var user = await _context.usuarios.FirstOrDefaultAsync(u => u.Correo == email);

        if (user == null)
        {
            throw new Exception("Usuario no encontrado con ese correo.");
        }

        // Genera un código aleatorio más friendly
        string code = GenerateRandomCode();

        // Encripta el código generado
        string salt;
        string passwordHash = Security.HashPassword(code, out salt);

        // Actualiza la clave del usuario con ese código encriptado
        await UpdatePasswordAsync(user.IdUsuario, passwordHash);

        // Envía el código por correo electrónico
        await SendEmailAsync(email, code);

        return code;
    }


    private string GenerateRandomCode()
    {
        // Lista de palabras seguras y agradables
        string[] palabras = new string[]
        {
        "Luna", "Sol", "Estrella", "Rio", "Bosque", "Montaña", "Brisa", "Nube", "Flor", "Mar",
        "Rayo", "Fuego", "Trueno", "Viento", "Agua", "Cielo", "Hoja", "Campo", "Nieve", "Arena"
        };

        Random random = new Random();

        // Elegimos dos palabras aleatorias
        string palabra1 = palabras[random.Next(palabras.Length)];
        string palabra2 = palabras[random.Next(palabras.Length)];

        // Generamos 3 números aleatorios
        int numeros = random.Next(100, 1000); // Asegura tres dígitos

        // Concatenamos el código
        return $"{palabra1}{palabra2}{numeros}";
    }


    private async Task SendEmailAsync(string email, string code)
    {
        var message = new MimeMessage();

        // Corregimos la creación de MailboxAddress utilizando la sobrecarga correcta
        message.From.Add(new MailboxAddress("Gestor Presupuestos", "noreply@ahm-honduras.com"));
        message.To.Add(new MailboxAddress(email, email)); // Asegúrate de que el correo se pase correctamente.

        message.Subject = "Recuperación de cuenta";

        // Cuerpo del mensaje en HTML
        string bodyHtml = $@"
    <html>
        <head>
            <style>
                body {{
                    font-family: Arial, sans-serif;
                    background-color: #f4f4f4;
                    color: #333333;
                }}
                .header {{
                    background-color:#282c3c;
                    color: white;
                    text-align: center;
                    padding: 10px;
                    font-size: 18px;
                }}
                .footer {{
                    background-color: #282c3c;
                    color: white;
                    text-align: center;
                    padding: 10px;
                    font-size: 14px;
                    position: absolute;
                    width: 100%;
                    bottom: 0;
                }}
                .content {{
                    padding: 20px;
                    text-align: center;
                }}
                .code {{
                    font-size: 24px;
                    font-weight: bold;
                    color: #282c3c;
                }}
            </style>
        </head>
        <body>
            <div class='header'>
                <h1>Gestion Presupuestaria AHM</h1>
            </div>
            <div class='content'>
                <p>Su clave temporal es:</p>
                <p class='code'>{code}</p>
            </div>
            <div class='footer'>
                <p>Recuerde cambiarla por su clave personal lo mas pronto posible.</p>
            </div>
        </body>
    </html>";

        message.Body = new TextPart("html")
        {
            Text = bodyHtml
        };

        using (var client = new SmtpClient())
        {
            try
            {
                // Conexión al servidor SMTP (aseguramos que el puerto y seguridad estén correctos)
                await client.ConnectAsync("smtp.office365.com", 587, false); // Usamos el servidor de Office365
                await client.AuthenticateAsync("noreply@ahm-honduras.com", "nr.ahm2012"); // Autenticación del correo

                // Envío del mensaje
                await client.SendAsync(message);
                await client.DisconnectAsync(true);
            }
            catch (Exception ex)
            {
                // Manejo de errores
                throw new Exception("Hubo un problema al enviar el correo: " + ex.Message);
            }
        }
    }





}
