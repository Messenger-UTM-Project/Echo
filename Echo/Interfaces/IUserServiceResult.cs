using System.Collections.Generic;

using Echo.Models;

namespace Echo.Interfaces
{
	public interface IUserServiceResult
	{
		int StatusCode { get; }
		List<User>? Users { get; }
	}
}
