using System.Net;
using AuthService.Application.Services;
using AuthService.Application.Services.Interfaces;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;
using UserService.API.Endpoints;
using UserService.API.Extensions;
using UserService.API.Middlewares;
using UserService.Application.Services;
using UserService.Application.Services.Interfaces;
using UserService.Application.Validators;
using UserService.Core.Models;
using UserService.Core.Repositories;
using UserService.Infrastructure.Services;
using UserService.Persistence.Contexts;
using UserService.Persistence.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

builder.WebHost.ConfigureKestrel(opts =>
{
    opts.Listen(IPAddress.Any, 5064, listenOptions =>
    {
        listenOptions.Protocols = HttpProtocols.Http1;
    });
    opts.Listen(IPAddress.Any, 50051, listenOptions =>
    {
        listenOptions.Protocols = HttpProtocols.Http2;
    });
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<GlobalExceptionsMiddleware>();

builder.Services.AddDbContext<UserDbContext>(opts =>
{
    opts.UseSqlServer(builder.Configuration.GetConnectionString("ccdb-users"));
    /*opts.UseInMemoryDatabase("UsersInMemo");*/
});

builder.Services.AddGrpc();

builder.Services.AddScoped<IRepository<User>, UserRepository>();
builder.Services.AddScoped<IRepository<Role>, RolesRepository>();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddScoped<UserCreateValidator>();
builder.Services.AddScoped<UserPatchValidator>();

builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<IBlobService, BlobService>();

builder.Services.AddUserCases();
builder.Services.AddRoleCases();

var app = builder.Build();

app.UseCors();

app.MapGrpcService<UserGrpcService>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

/*app.UseMiddleware<GlobalExceptionsMiddleware>();*/

app.MapUserEndpoints();
app.MapRoleEndpoint();

app.Run();
