using Microsoft.AspNetCore.Mvc;
using UserService.Application.Data.Dtos;
using UserService.Application.UseCases.UserCases;
using UserService.Application.Validators;

namespace UserService.API.Endpoints
{
    public static class UserEndpoints
    {
        public static IEndpointRouteBuilder MapUserEndpoints(this IEndpointRouteBuilder app)
        {
            var usersGroup = app.MapGroup("users");
            usersGroup.MapGet("/", GetAllUsers)
                .WithOpenApi()
                .WithSummary(nameof(GetAllUsers))
                .WithDescription("Returns a list of all users");

            usersGroup.MapGet("{id:guid}", GetUserById)
                .WithOpenApi()
                .WithSummary(nameof(GetUserById))
                .WithDescription("Returns a specified user by id");

            usersGroup.MapGet("{username}/exists", Exists)
                .WithOpenApi()
                .WithSummary(nameof(Exists))
                .WithDescription("Checks if user exists");

            usersGroup.MapPost("/", AddUser)
                .WithOpenApi()
                .WithSummary(nameof(AddUser))
                .WithDescription("Creates new user");

            usersGroup.MapPost("{userId:guid}/image", UploadProfileImage)
                .WithOpenApi()
                .Accepts<IFormFile>("multipart/form-data")
                .WithSummary(nameof(UploadProfileImage))
                .WithDescription("Lets user to upload profile image")
                .DisableAntiforgery();

            usersGroup.MapDelete("{id:guid}", DeleteUser)
                .WithOpenApi()
                .WithSummary(nameof(DeleteUser))
                .WithDescription("Deletes the specified user by id");

            usersGroup.MapPatch("{id:guid}", EditUser)
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

            var response = new ExistsDto()
            {
                Found = id is not null,
                Data = id
            };

            return Results.Ok(response);
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

        private static async Task<IResult> UploadProfileImage(
            Guid userId,
            [FromForm] IFormFile image,
            UploadProfileImage handler)
        {
            if (image.Length == 0)
            {
                return Results.BadRequest("Uploaded file is invalid");
            }

            var result = await handler.Invoke(userId, image);

            return result.Match(
                res => Results.Ok(res),
                err => Results.BadRequest(err));
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
