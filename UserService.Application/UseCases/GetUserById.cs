using AutoMapper;
using UserService.Application.Data.Dtos;
using UserService.Core.Exceptions;
using UserService.Core.Repositories;

namespace UserService.Application.UseCases
{
    public class GetUserById
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public GetUserById(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<UserReadDto> Invoke(Guid id)
        {
            var user = await _userRepository.GetByIdAsync(id) 
                ?? throw new UserServiceException(
                    UserServiceErrorTypes.ENTITY_NOT_FOUND,
                    "User not found");

            var dto = _mapper.Map<UserReadDto>(user);

            return dto;
        }
    }
}
