using System.Security.Claims;
using Microsoft.AspNetCore.Identity;

using Echo.Models;

namespace Echo.Interfaces
{
	public interface IAuthService
	{
		Task<ServiceResult<SignInResult>> AuthenticateAsync(string username, string password);
		Task<bool> LogoutAsync();
		bool IsAuthenticated(ClaimsPrincipal user);
	}
}
