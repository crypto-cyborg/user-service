using UserService.Application.UseCases;

namespace UserService.API.Extensions
{
    public static class ServiceExtensions
    {
        public static void AddUseCases(this IServiceCollection services)
        {
            services.AddScoped<GetAllUsers>();
            services.AddScoped<GetUserById>();
            services.AddScoped<Exists>();

            services.AddScoped<CreateUser>();

            services.AddScoped<DeleteUser>();

            services.AddScoped<PatchUser>();
        }
    }
}
