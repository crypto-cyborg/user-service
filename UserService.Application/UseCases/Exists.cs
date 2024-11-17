using UserService.Core.Exceptions;
using UserService.Core.Models;
using UserService.Core.Repositories;

namespace UserService.Application.UseCases
{
    public class Exists
    {
        private readonly IRepository<User> _repository;

        public Exists(IRepository<User> repository)
        {
            _repository = repository;
        }

        public async Task<Guid?> Invoke(string username)
        {
            var user = (await _repository.GetAsync(u => u.Username == username))
                .FirstOrDefault();

            if (user is null)
            {
                return null;
            }

            return user.Id;
        }
    }
}
