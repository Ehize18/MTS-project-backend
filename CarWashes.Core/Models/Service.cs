namespace CarWashes.Core.Models
{
	public class Service
	{
		public int? Id { get; }
		public int CarwashId { get; }
		public string Name { get; } = string.Empty;
		public decimal Price { get; }
		public TimeSpan Duration { get; }

		public Service(int? id, int carwashId, string name, decimal price, TimeSpan duration)
		{
			Id = id;
			CarwashId = carwashId;
			Name = name;
			Price = price;
			Duration = duration;
		}
	}
}
