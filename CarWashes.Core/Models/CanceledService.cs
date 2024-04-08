namespace CarWashes.Core.Models
{
	public class CanceledService
	{
		public int ServiceInOrderId { get; }
		public string Reason { get; } = string.Empty;

		public CanceledService(int serviceInOrderId, string reason)
		{
			ServiceInOrderId = serviceInOrderId;
			Reason = reason;
		}
	}
}
