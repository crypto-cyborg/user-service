namespace UserService.Core.Exceptions
{
    public class UserServiceException(UserServiceErrorTypes type, string message)
        : Exception(message)
    {
        public UserServiceErrorTypes Type { get; set; } = type;
    }
}
