using UserService.Application.Data.Dtos;
using UserService.Application.Services.Interfaces;
using UserService.Application.UseCases.RoleCases;

namespace UserService.API.Endpoints;

public static class RoleEndpoints
{
    public static IEndpointRouteBuilder MapRoleEndpoint(this IEndpointRouteBuilder app)
    {
        var roleGroup = app.MapGroup("roles");
        roleGroup.MapGet("/", GetAllRoles)
            .WithOpenApi()
            .WithSummary(nameof(GetAllRoles))
            .WithDescription("Returns a list of roles");

        roleGroup.MapPost("/", CreateRole)
            .WithOpenApi()
            .WithSummary(nameof(CreateRole))
            .WithDescription("Creates new app role");
        
        roleGroup.MapPatch("{userId:guid}/provide", Grant)
            .WithOpenApi()
            .WithSummary(nameof(Grant))
            .WithDescription("Provides a role to the user");

        roleGroup.MapPatch("{userId:guid}/revoke", Revoke)
            .WithOpenApi()
            .WithSummary(nameof(Revoke))
            .WithDescription("Revokes a role from the user");
        
        return app;
    }

    private static async Task<IResult> GetAllRoles(
        GetAllRoles handler)
    {
        return Results.Ok(await handler.Invoke());
    }

    private static async Task<IResult> Grant(
        Guid userId,
        int roleId,
        GrantRole handler)
    {
        var res = await handler.Invoke(userId, roleId);

        return res.Match(
            user => Results.Ok(user),
            error => Results.BadRequest(error));
    }

    private static async Task<IResult> Revoke(
        Guid userId,
        int roleId,
        RevokeRole handler)
    {
        var res = await handler.Invoke(userId, roleId);
        
        return res.Match(
            user => Results.Ok(user),
            error => Results.BadRequest(error));
    }

    private static async Task<IResult> CreateRole(
        RoleCreateDto request,
        CreateRole handler)
    {
        var res = await handler.Invoke(request);

        return res.Match(
            role => Results.Ok(role),
            err => Results.BadRequest(err));
    }
}
