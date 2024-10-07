using UserService.Core.Exceptions;
using UserService.Core.Repositories;

namespace UserService.Application.UseCases
{
    public class DeleteUser
    {
        private readonly IUserRepository _userRepository;

        public DeleteUser(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task Invoke(Guid id)
        {
            var user = await _userRepository.GetByIdAsync(id)
                ?? throw new EntityNotFoundException("Required user does not exist");

            _userRepository.Delete(user);

            await _userRepository.SaveAsync();

            _userRepository.Delete(user);
        }
    }
}
