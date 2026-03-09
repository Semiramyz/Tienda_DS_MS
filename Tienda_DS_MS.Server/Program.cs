using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Tienda_DS_MS.Server;
using Tienda_DS_MS.Server.Data;
using Tienda_DS_MS.Server.Services;

var builder = WebApplication.CreateBuilder(args);

// =============================================
// Bases de datos MySQL (una por dominio)
// =============================================
var serverVersion = new MySqlServerVersion(new Version(8, 0, 44));

var authCs = builder.Configuration.GetConnectionString("AuthDb")!;
builder.Services.AddDbContext<AuthDbContext>(o =>
    o.UseMySql(authCs, serverVersion, mysql => mysql.EnableRetryOnFailure()));

var clientesCs = builder.Configuration.GetConnectionString("ClientesDb")!;
builder.Services.AddDbContext<ClientesDbContext>(o =>
    o.UseMySql(clientesCs, serverVersion, mysql => mysql.EnableRetryOnFailure()));

var proveedoresCs = builder.Configuration.GetConnectionString("ProveedoresDb")!;
builder.Services.AddDbContext<ProveedoresDbContext>(o =>
    o.UseMySql(proveedoresCs, serverVersion, mysql => mysql.EnableRetryOnFailure()));

var productosCs = builder.Configuration.GetConnectionString("ProductosDb")!;
builder.Services.AddDbContext<ProductosDbContext>(o =>
    o.UseMySql(productosCs, serverVersion, mysql => mysql.EnableRetryOnFailure()));

var ventasCs = builder.Configuration.GetConnectionString("VentasDb")!;
builder.Services.AddDbContext<VentasDbContext>(o =>
    o.UseMySql(ventasCs, serverVersion, mysql => mysql.EnableRetryOnFailure()));

var facturasCs = builder.Configuration.GetConnectionString("FacturasDb")!;
builder.Services.AddDbContext<FacturasDbContext>(o =>
    o.UseMySql(facturasCs, serverVersion, mysql => mysql.EnableRetryOnFailure()));

var contabilidadCs = builder.Configuration.GetConnectionString("ContabilidadDb")!;
builder.Services.AddDbContext<ContabilidadDbContext>(o =>
    o.UseMySql(contabilidadCs, serverVersion, mysql => mysql.EnableRetryOnFailure()));

// =============================================
// JWT Authentication
// =============================================
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
        };
    });
builder.Services.AddAuthorization();

// =============================================
// Servicios de negocio
// =============================================
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IClienteService, ClienteService>();
builder.Services.AddScoped<IProveedorService, ProveedorService>();
builder.Services.AddScoped<IProductoService, ProductoService>();
builder.Services.AddScoped<IVentaService, VentaService>();
builder.Services.AddScoped<IFacturaService, FacturaService>();
builder.Services.AddScoped<IContabilidadService, ContabilidadService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapFallbackToFile("/index.html");

// Seed: crea el usuario administrador inicial
await DbSeeder.SeedAsync(app.Services);

app.Run();
