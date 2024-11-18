using UserService.Core.Models;
using UserService.Core.Repositories;

namespace UserService.Application.UseCases.UserCases
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

            return user?.Id;
        }
    }
}
