using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Echo.Models
{
	public class LoginViewModel
	{
		// Properties from SignUpViewModel
		[Display(Name = "Full Name")]
		public string? Name { get; set; }

		[Display(Name = "Username")]
		public string? Username { get; set; }

		[DataType(DataType.Password)]
		[Display(Name = "Password")]
		public string? Password { get; set; }

		[DataType(DataType.Password)]
		[Display(Name = "Confirm Password")]
		[Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
		public string? ConfirmPassword { get; set; }

		// Properties from SignInViewModel
		[Display(Name = "Username")]
		public string? SignInUsername { get; set; }

		[DataType(DataType.Password)]
		[Display(Name = "Password")]
		public string? SignInPassword { get; set; }

		[Display(Name = "Remember me")]
		public bool? RememberMe { get; set; }
	}
}
