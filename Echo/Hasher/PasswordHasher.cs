using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

using BCrypt.Net;

using Echo.Models;

namespace Echo.Hashers
{
	public class MyPasswordHasher : IPasswordHasher<User>
	{
		public string HashPassword(User user, string password)
		{
			// Ваш собственный метод хэширования пароля
			return BCrypt.Net.BCrypt.HashPassword(password);
		}

		public PasswordVerificationResult VerifyHashedPassword(User user, string hashedPassword, string providedPassword)
		{
			// Ваш собственный метод проверки пароля
			return BCrypt.Net.BCrypt.Verify(providedPassword, hashedPassword)
				? PasswordVerificationResult.Success
				: PasswordVerificationResult.Failed;
		}
	}
}
