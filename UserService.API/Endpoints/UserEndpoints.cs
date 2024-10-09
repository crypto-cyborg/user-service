using UserService.Application.Data.Dtos;
using UserService.Application.UseCases;
using UserService.Application.Validators;
using UserService.Core.Repositories;

namespace UserService.Endpoints
{
    public static class UserEndpoints
    {
        public static IEndpointRouteBuilder MapUserEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapGet("users", GetAllUsers)
                .WithOpenApi()
                .WithSummary(nameof(GetAllUsers))
                .WithDescription("Returns a list of all users");

            app.MapGet("users/{id}", GetUserById)
                .WithOpenApi()
                .WithSummary(nameof(GetUserById))
                .WithDescription("Returns a specified user by id");

            app.MapGet("users/{username}/exists", Exists)
                .WithOpenApi()
                .WithSummary(nameof(Exists))
                .WithDescription("Checks if user exists");

            app.MapPost("users", AddUser)
                .WithOpenApi()
                .WithSummary(nameof(AddUser))
                .WithDescription("Creates new user");

            app.MapDelete("users/{id}", DeleteUser)
                .WithOpenApi()
                .WithSummary(nameof(DeleteUser))
                .WithDescription("Deletes the specified user by id");

            app.MapPatch("users/{id}", EditUser)
                .WithOpenApi()
                .WithSummary(nameof(EditUser))
                .WithDescription("Edits the specified user");

            return app;
        }

        #region GET
        private static async Task<IResult> GetAllUsers(
            GetAllUsers handler)
        {
            return Results.Ok(await handler.Invoke());
        }

        private static async Task<IResult> GetUserById(
            Guid id,
            GetUserById handler)
        {
            var user = await handler.Invoke(id);

            return Results.Ok(user);
        }

        private static async Task<IResult> Exists(
            string username,
            Exists handler)
        {
            var id = await handler.Invoke(username);

            return Results.Ok(id is null ? 0 : id);
        }
        #endregion

        #region POST
        private static async Task<IResult> AddUser(
            UserCreateDto request,
            CreateUser handler)
        {
            var response = await handler.Invoke(request);

            return Results.Ok(response);
        }
        #endregion

        #region DELETE
        private static async Task<IResult> DeleteUser(
            Guid id,
            DeleteUser hander)
        {
            await hander.Invoke(id);

            return Results.NoContent();
        }
        #endregion

        #region Patch
        private static async Task<IResult> EditUser(
            Guid id,
            UserPatchDto request,
            PatchUser handler,
            UserPatchValidator validator)
        {
            var validationResult = await validator.ValidateAsync(request);

            if (!validationResult.IsValid)
            {
                return Results.BadRequest(validationResult.Errors);
            }

            var patchedUser = await handler.Invoke(id, request);

            return Results.Ok(patchedUser);
        }
        #endregion
    }
}
