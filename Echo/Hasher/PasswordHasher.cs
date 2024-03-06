using Microsoft.AspNetCore.Identity;

using Echo.Models;

namespace Echo.Hashers
{
    public class MyPasswordHasher : IPasswordHasher<User>
	{
		public string HashPassword(User user, string password)
		{
			return BCrypt.Net.BCrypt.HashPassword(password);
		}

		public PasswordVerificationResult VerifyHashedPassword(User user, string hashedPassword, string providedPassword)
		{
			return BCrypt.Net.BCrypt.Verify(providedPassword, hashedPassword)
				? PasswordVerificationResult.Success
				: PasswordVerificationResult.Failed;
		}
	}
}
