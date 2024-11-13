using AutoMapper;
using FluentValidation;
using UserService.Application.Data.Dtos;
using UserService.Application.Validators;
using UserService.Core.Exceptions;
using UserService.Core.Models;
using UserService.Core.Repositories;

namespace UserService.Application.UseCases
{
    public class CreateUser
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly UserCreateValidator _validator;

        public CreateUser(
            IUserRepository userRepository,
            IMapper mapper,
            UserCreateValidator validator)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _validator = validator;
        }

        public async Task<UserReadDto> Invoke(UserCreateDto request)
        {
            var validationResult = _validator.Validate(request);

            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            if ((await _userRepository.Get(u => u.Username == request.Username)).Any())
            {
                throw new UserServiceException(
                    UserServiceErrorTypes.INVALID_USERNAME,
                    "Username should be unique");
            }

            if ((await _userRepository.Get(u => u.Email == request.Email)).Any())
            {
                throw new UserServiceException(
                    UserServiceErrorTypes.INVALID_EMAIL,
                    "Username should be unique");
            }

            var user = _mapper.Map<User>(request);
            user.PasswordHash = BCrypt.Net.BCrypt.EnhancedHashPassword(request.Password);

            await _userRepository.Insert(user);
            await _userRepository.SaveAsync();

            return _mapper.Map<UserReadDto>(user);
        }
    }
}
