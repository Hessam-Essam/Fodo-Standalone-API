using Fodo.Application.Implementation.Interfaces;
using Fodo.Application.Implementation.IRepositories;
using Fodo.Application.Implementation.Services;
using Fodo.Domain.Entities;
using Fodo.Infrastructure.Persistence;
using Fodo.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore; // Add this using directive
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using System;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<Fodo.Infrastructure.Persistence.IdentityDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("FoodoDatabase")));
// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddScoped<IBranchesService, BranchesService>();
builder.Services.AddScoped<IDeviceVerificationService, DeviceVerificationService>();
builder.Services.AddScoped<IBranchesRepository, BranchesRepository>();
builder.Services.AddScoped<IClientRepository, ClientRepository>();
builder.Services.AddScoped<IDeviceRepository, DeviceRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ITokenService, TokenService>();

// choose one:
builder.Services.AddScoped<IPasswordService, PasswordService>();
//builder.Services.AddHttpClient<IClientsService, ClientsService>(http =>
//{
//    http.BaseAddress = new Uri(builder.Configuration["Services:ClientsBaseUrl"]!);
//});
builder.Services.AddControllersWithViews();
builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddScoped<IAuthService,
//                          Fodo.Application.Implementation.Services.AuthService>();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "My API",
        Version = "v1",
        Description = ".NET 10 Clean Architecture API"
    });
});
builder.Services.AddIdentity<User, IdentityRole>(options =>
    {
        // options.Password..., options.SignIn..., etc.
    })
    .AddEntityFrameworkStores<Fodo.Infrastructure.Persistence.IdentityDbContext>() // Ensure this is called after AddIdentity
    .AddDefaultTokenProviders();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AppCors", policy =>
        policy.WithOrigins("https://lightstandalone.fodo.app:8443/")   // your frontend origin
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials());
});
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
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])
            ),

            ClockSkew = TimeSpan.Zero
        };
    });
builder.Services.AddAuthorization();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();


var app = builder.Build();
app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto | ForwardedHeaders.XForwardedHost
});
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "My API v1");
        options.RoutePrefix = "swagger"; // URL: /swagger
    });
    app.MapOpenApi();
}
if (!app.Environment.IsDevelopment())
{
    // Only if your proxy forwards X-Forwarded-Proto correctly (with UseForwardedHeaders)
    app.UseHttpsRedirection();
}
app.UseSwagger(c =>
{
    c.PreSerializeFilters.Add((swagger, httpReq) =>
    {
        swagger.Servers = new List<OpenApiServer>
        {
            new OpenApiServer { Url = $"{httpReq.Scheme}://{httpReq.Host.Value}" }
        };
    });
});
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "My API v1");
    options.RoutePrefix = "swagger"; // URL: /swagger
});
app.UseSwagger();
app.UseHttpsRedirection();
app.UseForwardedHeaders();
app.UseRouting();
app.UseCors("AppCors");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
