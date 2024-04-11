namespace API.Authorization
{
    public enum AuthError
    {
        WrongUsername,
        WrongPassword
    }

    public class AuthException(AuthError error) : Exception(error.ToString())
    {
        public AuthError Error { get; } = error;
    }

}
