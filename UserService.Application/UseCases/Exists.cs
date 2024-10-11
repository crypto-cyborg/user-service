using UserService.Core.Exceptions;
using UserService.Core.Repositories;

namespace UserService.Application.UseCases
{
    public class Exists
    {
        private readonly IUserRepository _userRepository;

        public Exists(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<Guid?> Invoke(string username)
        {
            var user = (await _userRepository.Get(u => u.Username == username))
                .FirstOrDefault();

            return user.Id;
        }
    }
}
