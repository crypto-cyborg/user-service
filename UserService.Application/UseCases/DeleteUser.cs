using UserService.Core.Exceptions;
using UserService.Core.Models;
using UserService.Core.Repositories;

namespace UserService.Application.UseCases
{
    public class DeleteUser
    {
        private readonly IRepository<User> _repository;

        public DeleteUser(IRepository<User> repository)
        {
            _repository = repository;
        }

        public async Task Invoke(Guid id)
        {
            var user = await _repository.GetByIdAsync(id)
                ?? throw new UserServiceException(
                    UserServiceErrorTypes.ENTITY_NOT_FOUND, 
                    "Required user does not exist");

            _repository.Delete(user);

            await _repository.SaveAsync();

            _repository.Delete(user);
        }
    }
}
