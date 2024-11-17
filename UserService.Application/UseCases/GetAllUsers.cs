using AutoMapper;
using UserService.Application.Data.Dtos;
using UserService.Application.UseCases.Interfaces;
using UserService.Core.Models;
using UserService.Core.Repositories;

namespace UserService.Application.UseCases
{
    public class GetAllUsers
    {
        private readonly IRepository<User> _repository;
        private readonly IMapper _mapper;

        public GetAllUsers(
            IRepository<User> repository,
            IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<UserReadDto>> Invoke()
        {
            var users = await _repository.GetAsync();
            var dtos = _mapper.Map<IEnumerable<UserReadDto>>(users);

            return dtos;
        }
    }
}
