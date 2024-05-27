namespace Echo.Interfaces
{
	public interface IUserConnectionManager
	{
		void KeepUserConnection(Guid userUuid, string connectionId);
		void RemoveUserConnection(Guid userUuid, string connectionId);
		string GetConnectionId(Guid userUuid);
	}
}
