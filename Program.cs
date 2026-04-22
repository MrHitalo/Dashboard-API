using Dashboard.Data;
using Dashboard.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// ========== SERVICES ==========

// Configuração do banco de dados InMemory (equivalente ao SQLite/MySQL do Laravel)
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseInMemoryDatabase("DashboardDb"));

// Configuração do JWT
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey = jwtSettings["SecretKey"] ?? throw new InvalidOperationException("JWT SecretKey não configurada");

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
    };
});

// Registrar serviços
builder.Services.AddScoped<IJwtService, JwtService>();

// Controllers
builder.Services.AddControllers();

// OpenAPI/Swagger
builder.Services.AddOpenApi();

// CORS (permitir requisições do frontend)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// ========== MIDDLEWARE PIPELINE ==========

// Swagger apenas em desenvolvimento
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

// CORS
app.UseCors("AllowAll");

// HTTPS Redirect
app.UseHttpsRedirection();

// Autenticação e Autorização (ORDEM IMPORTA!)
app.UseAuthentication();  // Verifica o token JWT
app.UseAuthorization();   // Verifica permissões

// Mapear controllers
app.MapControllers();

app.Run();
