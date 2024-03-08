namespace Echo.Interfaces
{
	public interface IUserServiceResult<T> : IStatusCode where T : class
	{
		T Result { get; set; }
	}
}
