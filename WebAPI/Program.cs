using Autofac.Extensions.DependencyInjection;
using Autofac;
using DataAccess.Concrete.EntityFramework.Contexts;
using Microsoft.EntityFrameworkCore;
using Business.DependencyResolvers.Autofac;
using Core.Utilities.Security.Jwt;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Core.Utilities.Security.Encyption;
using Autofac.Core;
using Business.ValidationRules.FluentValidation;
using FluentValidation;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.AddCors(p => p.AddPolicy("AllowOrigin", policy =>
{
    policy
        .WithOrigins(
            "http://localhost:3000",   // React dev server (http)
            "https://localhost:3000",  // React dev server (https)
            "http://127.0.0.1:3000",   // bazen 127.0.0.1 olarak �al���r
            "http://localhost:5173",
            "http://127.0.0.1:5173",
            "https://localhost:7007"   // Swagger veya backend test icin
        )
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials();
}));


var tokenOptions = builder.Configuration.GetSection("TokenOptions").Get<TokenOptions>();

builder.Services.AddValidatorsFromAssemblyContaining<MarketValidator>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            ValidIssuer = tokenOptions.Issuer,
            ValidAudience = tokenOptions.Audience,
            IssuerSigningKey = SecurityKeyHelper.CreateSecurityKey(tokenOptions.SecurityKey),

            ClockSkew = TimeSpan.Zero // s�re doldu�unda hemen ge�ersiz olsun
        };
    });

//Message �ifreleme - key: 32 byte (AES-256)
builder.Services.AddSingleton<byte[]>(sp =>
{
    var cfg = sp.GetRequiredService<IConfiguration>();
    var keyB64 = cfg.GetValue<string>("Crypto:KeyBase64")
    ?? throw new Exception("Crypto:KeyBase64 ayarı bulunamadı.");
    if (string.IsNullOrWhiteSpace(keyB64))
        throw new Exception("Crypto:KeyBase64 ayarı bulunamadı.");

    var key = Convert.FromBase64String(keyB64);
    if (key.Length != 32)
        throw new Exception("Crypto key 32 byte (AES-256) olmalı.");

    return key;
});

// ?? Default DI yerine Autofac kullanaca��m�z� belirtiyoruz
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());

// ?? Autofac Mod�l�n� y�kl�yoruz
builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder =>
{
    containerBuilder.RegisterModule(new AutofacBusinessModule());
});


// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowOrigin");

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    Console.WriteLine("✅ PostgreSQL provider: " + db.Database.ProviderName);
}

app.Run();
