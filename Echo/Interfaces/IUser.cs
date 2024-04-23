
namespace Echo.Interfaces
{
	interface IUser : ITrackingEntity
	{
		bool IsActive { get; set; }
        DateTime CreatedAt { get; set; }
	}
}
