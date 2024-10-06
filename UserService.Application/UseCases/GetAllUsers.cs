using AutoMapper;
using UserService.Application.Data.Dtos;
using UserService.Application.UseCases.Interfaces;
using UserService.Core.Repositories;

namespace UserService.Application.UseCases
{
    public class GetAllUsers
    {
        private readonly IUserRepository _repository;
        private readonly IMapper _mapper;

        public GetAllUsers(
            IUserRepository repository,
            IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<UserReadDto>> Invoke()
        {
            var users = await _repository.GetAllAsync();
            var dtos = _mapper.Map<IEnumerable<UserReadDto>>(users);

            return dtos;
        }
    }
}
