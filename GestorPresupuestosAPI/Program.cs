using GestorPresupuestosAPI.Features.Repository;
using GestorPresupuestosAPI.Features.Services;
using GestorPresupuestosAPI.Infraestructure.DataBases;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InvariantCulture;
CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.InvariantCulture;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
         builder =>
         {
             builder.WithOrigins("http://localhost:5173", "https://www.ahm-honduras.com")
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials();
         });
});
builder.Services.AddDbContext<GestorPresupuestosAHM>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<UsuarioRepository>();
builder.Services.AddScoped<DepartamentoRepository>();
builder.Services.AddScoped<CategoriaRepository>();
builder.Services.AddScoped<SubcategoriaRepository>();
builder.Services.AddScoped<PresupuestoRepository>();
builder.Services.AddScoped<CuentaRepository>();
builder.Services.AddScoped<PresupuestoCuentaRepository>();
builder.Services.AddScoped<ProveedoresRepository>();
builder.Services.AddScoped<NotificacionesRepository>();
builder.Services.AddScoped<DashboardRepository>();
builder.Services.AddScoped<ServicioRepository>();


builder.Services.AddScoped<UsuarioService>();
builder.Services.AddScoped<DepartamentoService>();
builder.Services.AddScoped<CategoriaService>();
builder.Services.AddScoped<SubcategoriaService>();
builder.Services.AddScoped<PresupuestoService>();
builder.Services.AddScoped<CuentaService>();
builder.Services.AddScoped<PresupuestoCuentaService>();
builder.Services.AddScoped<ProveedoresService>();
builder.Services.AddScoped<NotificacionesService>();
builder.Services.AddScoped<ReportService>();
builder.Services.AddScoped<DashboardService>();
builder.Services.AddScoped<ServicioService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowSpecificOrigin");

app.UseAuthorization();

app.MapControllers();

app.Run();
