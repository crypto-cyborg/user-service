namespace UserService.Core.Utils
{
    public static class BCrypt
    {
        public static bool IsBCryptHash(this string input)
            => input.Length == 60 && (input.StartsWith("$2a$") || input.StartsWith("$2b$") || input.StartsWith("$2y$"));
    }
}
