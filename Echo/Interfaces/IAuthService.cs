using Microsoft.AspNetCore.Identity;

public interface IAuthService
{
    Task<SignInResult> AuthenticateAsync(string username, string password);
}
