using Echo.Enum;
using Echo.Interfaces;

namespace Echo.Models
{
    public class UserServiceResult<T> : IUserServiceResult<T> where T : class
	{
		public HttpStatusCode StatusCode { get; set; } = HttpStatusCode.OK;
		public T Result { get; set; } = default!;

		public UserServiceResult() { }

		public UserServiceResult(Action<UserServiceResult<T>> options)
		{
			options?.Invoke(this);
		}
	}
}
