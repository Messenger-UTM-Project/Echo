using Echo.Interfaces;

namespace Echo.Models
{
    public class UserServiceResult : IUserServiceResult
	{
		public int StatusCode { get; set; }
		public List<User>? Users { get; set; }
	}
}
