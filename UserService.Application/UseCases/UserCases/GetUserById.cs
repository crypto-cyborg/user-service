using AutoMapper;
using UserService.Application.Data.Dtos;
using UserService.Core.Exceptions;
using UserService.Core.Models;
using UserService.Core.Repositories;

namespace UserService.Application.UseCases.UserCases
{
    public class GetUserById
    {
        private readonly IRepository<User> _repository;
        private readonly IMapper _mapper;

        public GetUserById(IRepository<User> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<UserReadDto> Invoke(Guid id)
        {
            var user = await _repository.GetByIdAsync(id) 
                ?? throw new UserServiceException(
                    UserServiceErrorTypes.ENTITY_NOT_FOUND,
                    "User not found");

            var dto = _mapper.Map<UserReadDto>(user);

            return dto;
        }
    }
}
