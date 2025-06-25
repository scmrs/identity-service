﻿using Identity.API;
using Identity.Application;
using Identity.Application.Services;
using Identity.Infrastructure;
using Identity.Infrastructure.Data.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Security.Claims;
using System.Text;
using MassTransit;
using BuildingBlocks.Messaging.MassTransit;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddApplicationServices(builder.Configuration)
    .AddInfrastructureServices(builder.Configuration)
    .AddApiServices(builder.Configuration);
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
        ValidIssuer = "identity-service",
        ValidAudience = "webapp",
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("8f9c08c9e6bde3fc8697fbbf91d52a5dcd2f72f84b4b8a6c7d8f3f9d3db249a1")),
        RoleClaimType = ClaimTypes.Role
    };
});
builder.Services.AddCors();
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Admin", policy =>
        policy.RequireRole("Admin"));
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });

    // Cấu hình Bearer Token cho Swagger UI
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "bearer",
        BearerFormat = "JWT",
        Description = "Please enter token in the format: Bearer {your_token_here}"
    });
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
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));
// Añadir en Program.cs o archivo de configuración de servicios
builder.Services.Configure<ImageKitOptions>(builder.Configuration.GetSection("ImageKit"));
builder.Services.AddHttpClient<IImageKitService, ImageKitService>();
builder.Services.AddHttpClient("NotificationAPI", client =>
{
    client.BaseAddress = new Uri("https://localhost:7069");
});
builder.Logging.AddConsole();

var app = builder.Build();
app.UseCors(builder =>
{
    builder.AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader();
});
app.UseAuthentication();
app.UseAuthorization();
app.UseApiServices();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    await app.InitialiseDatabaseAsync();
}

app.Run();

public partial class Program
{ }