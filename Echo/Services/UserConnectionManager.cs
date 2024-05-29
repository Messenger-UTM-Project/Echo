using System.Collections.Concurrent;

using Echo.Interfaces;

namespace Echo.Services
{
	public class UserConnectionManager : IUserConnectionManager
	{
		private static readonly ConcurrentDictionary<Guid, string> userConnections = new();

		public void KeepUserConnection(Guid userUuid, string connectionId)
		{
			userConnections[userUuid] = connectionId;
		}

		public void RemoveUserConnection(Guid userUuid, string connectionId)
		{
			userConnections.TryRemove(userUuid, out _);
		}

		public string GetConnectionId(Guid userUuid)
		{
			userConnections.TryGetValue(userUuid, out var connectionId);
			return connectionId;
		}
	}
}
