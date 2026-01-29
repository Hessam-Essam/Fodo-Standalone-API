using Fodo.Application.Implementation.Interfaces;
using Fodo.Application.Implementation.IRepositories;
using Fodo.Application.Implementation.Services;
using Fodo.Domain.Entities;
using Fodo.Infrastructure.Persistence;
using Fodo.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore; // Add this using directive
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;
using System;

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
builder.Services
    .AddIdentity<User, IdentityRole>(options =>
    {
        // options.Password..., options.SignIn..., etc.
    })
    .AddEntityFrameworkStores<Fodo.Infrastructure.Persistence.IdentityDbContext>() // Ensure this is called after AddIdentity
    .AddDefaultTokenProviders();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
});
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();


var app = builder.Build();
app.UseRouting();
// Configure the HTTP request pipeline.
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
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "My API v1");
    options.RoutePrefix = "swagger"; // URL: /swagger
});
app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseAuthorization();

app.MapControllers();

app.Run();
