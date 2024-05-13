namespace CarWashes.Core.Models
{
	public class AvailableTimeForOrder
	{
		public int PostId { get; }
		public List<DateTime> AvailableTime { get; }

		public AvailableTimeForOrder(int postId, List<DateTime> availableTime)
		{
			PostId = postId;
			AvailableTime = availableTime;
		}
	}
}
