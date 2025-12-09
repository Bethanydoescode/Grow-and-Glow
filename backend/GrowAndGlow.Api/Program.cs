using GrowAndGlow.Api.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// --------------------------------------------------------
// DATABASE CONFIG — SQLite Local / Postgres When Provided
// --------------------------------------------------------
var connectionStringSqlite = builder.Configuration.GetConnectionString("SQLite");
var connectionStringPostgres = builder.Configuration.GetConnectionString("Postgres");
var usePostgres = !string.IsNullOrEmpty(connectionStringPostgres);

// Use SQLite locally (default)
if (!usePostgres)
{
    builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseSqlite(connectionStringSqlite));
}
else // Use Postgres automatically if connection string exists (deployed cloud)
{
    builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseNpgsql(connectionStringPostgres));
}

// --------------------------------------------------------
// CONTROLLERS, SWAGGER, CORS
// --------------------------------------------------------
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// CORS (allow React dev environment)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:5173")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

// --------------------------------------------------------
// JWT Authentication
// --------------------------------------------------------
var jwtKey = builder.Configuration["Jwt:Key"];
var jwtIssuer = builder.Configuration["Jwt:Issuer"];
var jwtAudience = builder.Configuration["Jwt:Audience"];

// 🔐 Validate Key BEFORE Using It
if (string.IsNullOrEmpty(jwtKey))
    throw new InvalidOperationException("❌ JWT Key is missing from configuration (appsettings.json / secrets).");

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtIssuer,
            ValidAudience = jwtAudience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
        };
    });

// --------------------------------------------------------
var app = builder.Build();

// --------------------------------------------------------
// MIDDLEWARE PIPELINE
// --------------------------------------------------------
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowFrontend");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
