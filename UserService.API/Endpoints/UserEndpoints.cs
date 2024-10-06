using UserService.Application.Data.Dtos;
using UserService.Application.UseCases;
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

            return app;
        }

        private static async Task<IResult> GetAllUsers(
            IUserRepository userRepository)
        {
            return Results.Ok(await userRepository.GetAllAsync());
        }

        private static async Task<IResult> GetUserById(
            Guid id,
            IUserRepository userRepository)
        {
            var user = await userRepository.GetByIdAsync(id);

            if (user is null)
            {
                return Results.NotFound($"User not found");
            }

            return Results.Ok(user);
        }

        private static async Task<IResult> Exists(
            string username,
            IUserRepository userRepository)
        {
            var user = (await userRepository.Get(u => u.Username == username)).First();

            if (user is null)
            {
                return Results.NotFound($"User does not exist");
            }

            return Results.Ok(user.Id);
        }

        private static async Task<IResult> AddUser(
            UserCreateDto request,
            CreateUser handler)
        {
            var response = await handler.Invoke(request);

            return Results.Ok(response);
        }
    }
}
