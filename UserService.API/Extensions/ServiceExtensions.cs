using UserService.Application.UseCases.RoleCases;
using UserService.Application.UseCases.UserCases;

namespace UserService.API.Extensions
{
    public static class ServiceExtensions
    {
        public static void AddUserCases(this IServiceCollection services)
        {
            services.AddScoped<GetAllUsers>();
            services.AddScoped<GetUserById>();
            services.AddScoped<Exists>();

            services.AddScoped<CreateUser>();

            services.AddScoped<DeleteUser>();

            services.AddScoped<PatchUser>();
        }

        public static void AddRoleCases(this IServiceCollection services)
        {
            services.AddScoped<GetAllRoles>();
            services.AddScoped<CreateRole>();
            services.AddScoped<GrantRole>();
            services.AddScoped<RevokeRole>();
        }
    }
}
