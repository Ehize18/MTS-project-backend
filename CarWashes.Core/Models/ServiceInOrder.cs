namespace CarWashes.Core.Models
{
	public class ServiceInOrder
	{
		public int Id { get; }
		public int OrderId { get; }
		public int ServiceId { get; }

		public ServiceInOrder(int id, int orderId, int serviceId)
		{
			Id = id;
			OrderId = orderId;
			ServiceId = serviceId;
		}
	}
}
