using Echo.Enum;
using Echo.Interfaces;

namespace Echo.Models
{
    public class ServiceResult<T> : IServiceResult<T> where T : class
	{
		public HttpStatusCode StatusCode { get; set; } = HttpStatusCode.OK;
		public T Result { get; set; } = default!;

		public ServiceResult() { }

		public ServiceResult(Action<ServiceResult<T>> options)
		{
			options?.Invoke(this);
		}
	}
}
