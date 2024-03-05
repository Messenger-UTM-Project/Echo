using System.Collections.Generic;

using Echo.Data;
using Echo.Models;
using Echo.Interfaces;

namespace Echo.Services 
{
	public class UserServiceResult : IUserServiceResult
	{
		public int StatusCode { get; set; }
		public List<User>? Users { get; set; }
	}
}
