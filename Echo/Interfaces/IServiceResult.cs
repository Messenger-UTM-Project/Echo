namespace Echo.Interfaces
{
	public interface IServiceResult<T> : IStatusCode where T : class
	{
		T Result { get; set; }
	}
}
