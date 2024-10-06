using System.ComponentModel.DataAnnotations;

namespace UserService.Application.Data.Dtos
{
    public record UserCreateDto(
        [Required] string Username,
        [Required] string Password,
        [Required] string ConfirmPassword,
        [Required] string Email,
        [Required] string FirstName,
        [Required] string LastName
    );
}
