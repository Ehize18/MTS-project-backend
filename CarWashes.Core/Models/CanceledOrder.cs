namespace CarWashes.Core.Models
{
	internal class CanceledOrder
	{
		public int OrderId { get; }
		public string Reason { get; } = string.Empty;

		public CanceledOrder(int orderId, string reason)
		{
			OrderId = orderId;
			Reason = reason;
		}
	}
}
