using AuthService.Application.Services.Interfaces;
using LanguageExt.Common;
using Microsoft.AspNetCore.Http;
using UserService.Application.Data.Dtos;
using UserService.Core.Exceptions;
using UserService.Core.Models;
using UserService.Core.Repositories;

namespace UserService.Application.UseCases.UserCases;

public class UploadProfileImage(IBlobService blobService, IRepository<User> userRepository)
{
    public async Task<Result<UploadProfileImageResponse>> Invoke(Guid userId, IFormFile image)
    {
        var user = await userRepository.GetByIdAsync(userId);

        if (user is null)
        {
            return new Result<UploadProfileImageResponse>(
                new UserServiceException(UserServiceErrorTypes.ENTITY_NOT_FOUND, "User does not exist"));
        }

        var url = await blobService.UploadImage(image);

        user.ImageUrl = url;
        await userRepository.SaveAsync();

        var result = new UploadProfileImageResponse(user.Id, user.ImageUrl);

        return result;
    }
}