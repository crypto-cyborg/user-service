﻿using AutoMapper;
using UserService.Application.Data.Dtos;
using UserService.Core.Exceptions;
using UserService.Core.Repositories;
using UserService.Core.Utils;

namespace UserService.Application.UseCases
{
    public class PatchUser
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public PatchUser(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<UserReadDto> Invoke(Guid id, UserPatchDto request)
        {
            var user =
                await _userRepository.GetByIdAsync(id)
                ?? throw new UserServiceException(
                    UserServiceErrorTypes.ENTITY_NOT_FOUND,
                    "User not found"
                );

            if (request.Username is not null)
                user.Username = request.Username;

            if (request.PasswordHash is not null)
            {
                if (!request.PasswordHash.IsBCryptHash())
                {
                    request.PasswordHash = BCrypt.Net.BCrypt.EnhancedHashPassword(request.PasswordHash);
                }

                user.PasswordHash = request.PasswordHash;
            }

            if (request.Email is not null)
                user.Email = request.Email;

            user.IsEmailConfirmed = request.IsEmailConfirmed;

            if (request.FirstName is not null)
                user.FirstName = request.FirstName;

            if (request.LastName is not null)
                user.LastName = request.LastName;

            if (request.RefreshToken is not null)
                user.RefreshToken = request.RefreshToken;

            if (user.RefreshTokenExpiryTime < request.RefreshTokenExpiryTime)
            {
                user.RefreshTokenExpiryTime = request.RefreshTokenExpiryTime;
            }

            await _userRepository.SaveAsync();

            return _mapper.Map<UserReadDto>(user);
        }
    }
}
