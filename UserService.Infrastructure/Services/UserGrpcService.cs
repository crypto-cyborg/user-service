using System.Diagnostics;
using Grpc.Core;
using UserService.Core.Models;
using UserService.Core.Repositories;

namespace UserService.Infrastructure.Services;

public class UserGrpcService(IRepository<User> repository) : Users.UsersBase
{
    public override async Task<ExistsReply> FindByIdOrSlug(ExistsRequest request, ServerCallContext context)
    {
        var isId = Guid.TryParse(request.IdOrSlug, out var id);

        var user = isId
            ? await repository.GetByIdAsync(id)
            : (await repository.GetAsync(u => u.Username == request.IdOrSlug)).FirstOrDefault();
        
        return new ExistsReply
        {
            Found = user is not null,
            Id = user?.Id.ToString() ?? string.Empty,
            Username = user?.Username ?? string.Empty
        };
    }
}