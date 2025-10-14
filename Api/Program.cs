using DataAccess.Context;
using DataAccess.Repositories;
using Business.Services;
using Business.Mappers;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;
using DataAccess.Interfaces;
using Business.Interfaces;
using Api.Services;

var builder = WebApplication.CreateBuilder(args);

// ==========================================================
// SERVİS KAYITLARI (Dependency Injection)
// ==========================================================

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(connectionString);
});

// Katman Bağımlılıkları
builder.Services.AddScoped<IAuthRepository, AuthRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.Configure<Business.Settings.JwtSettings>(builder.Configuration.GetSection("JwtSettings"));


// Email Servisi Bağımlılıkları
builder.Services.Configure<Business.Settings.EmailSettings>(
    builder.Configuration.GetSection("EmailSettings"));

// AutoMapper
builder.Services.AddAutoMapper(typeof(MappingProfile));
// Controller
builder.Services.AddControllers();

// ==========================================================
//GÜVENLİK AYARLARI: JWT Kimlik Doğrulama
// ==========================================================

var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var key = Encoding.ASCII.GetBytes(jwtSettings["SecretKey"]!);
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidateAudience = true,
        ValidAudience = jwtSettings["Audience"],
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero
    };
});


// ==========================================================
// SWAGGER
// ==========================================================

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    // JWT için Güvenlik Tanımı Ekleniyor
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme. Enter 'Bearer' [space] and then your token in the text input below.",
    });

    // Swagger UI'da kilidi etkinleştir
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

// ==========================================================
// UYGULAMA MİDDLEWARE PIPELINE KURULUMU
// ==========================================================

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

// JWT Kimlik Doğrulama Middleware'ini ekle
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();