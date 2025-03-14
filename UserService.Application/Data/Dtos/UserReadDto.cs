﻿namespace UserService.Application.Data.Dtos
{
    public class UserReadDto
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public string? ImageUrl { get; set; }

        public string Email { get; set; }
        public bool IsEmailConfirmed { get; set; } = false;

        public string ApiKey { get; set; }
        public string SecretKey { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }

        public ICollection<RoleReadDto> Roles { get; set; }

        public string? RefreshToken { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }
    }
}
