using AutoMapper;
using FluentValidation;
using Microsoft.VisualBasic;
using UserService.Application.Data.Dtos;
using UserService.Application.Services.Interfaces;
using UserService.Application.Validators;
using UserService.Core.Exceptions;
using UserService.Core.Models;
using UserService.Core.Repositories;

namespace UserService.Application.UseCases.UserCases
{
    public class CreateUser(
        IRepository<User> userRepository,
        IMapper mapper,
        UserCreateValidator validator,
        IRoleService roleService,
        IRepository<Role> roleRepository)
    {
        public async Task<UserReadDto> Invoke(UserCreateDto request)
        {
            var validationResult = await validator.ValidateAsync(request);

            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            if ((await userRepository.GetAsync(u => u.Username == request.Username)).Any())
            {
                throw new UserServiceException(
                    UserServiceErrorTypes.INVALID_USERNAME,
                    "Username should be unique");
            }

            if ((await userRepository.GetAsync(u => u.Email == request.Email)).Any())
            {
                throw new UserServiceException(
                    UserServiceErrorTypes.INVALID_EMAIL,
                    "Username should be unique");
            }

            var user = mapper.Map<User>(request);
            user.PasswordHash = BCrypt.Net.BCrypt.EnhancedHashPassword(request.Password);

            var roleToApply = await roleRepository.GetAsync(r => r.Name == "AppUser");

            await userRepository.InsertAsync(user);
            await roleService.Grant(user, roleToApply.First());
            await userRepository.SaveAsync();

            return mapper.Map<UserReadDto>(user);
        }
    }
}