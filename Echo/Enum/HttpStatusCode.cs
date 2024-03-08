using System;
using System.ComponentModel.DataAnnotations;

namespace Echo.Enum
{
	public enum HttpStatusCode
	{
		[Display(Name = "Continue")]
		Continue = 100,
		[Display(Name = "OK")]
		OK = 200,
		[Display(Name = "Bad Request")]
		BadRequest = 400,
		[Display(Name = "Unauthorized")]
		Unauthorized = 401,
		[Display(Name = "Forbidden")]
		Forbidden = 403,
		[Display(Name = "Not Found")]
		NotFound = 404,
	}
}
