using Microsoft.EntityFrameworkCore;
using UserService.API.Extensions;
using UserService.API.Middlewares;
using UserService.Application.Validators;
using UserService.Core.Repositories;
using UserService.Endpoints;
using UserService.Persistence.Contexts;
using UserService.Persistence.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<GlobalExceptionsMiddleware>();

builder.Services.AddDbContext<UserDbContext>(opts =>
{
    opts.UseInMemoryDatabase("UsersInMemo");
});

builder.Services.AddScoped<IUserRepository, UserRepository>();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddScoped<UserCreateValidator>();
builder.Services.AddScoped<UserPatchValidator>();

builder.Services.AddUseCases();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseMiddleware<GlobalExceptionsMiddleware>();

app.MapUserEndpoints();

app.Run();
