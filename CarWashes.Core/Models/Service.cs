namespace CarWashes.Core.Models
{
	public class Service
	{
		public int Id { get; }
		public string Name { get; } = string.Empty;
		public decimal Price { get; }
		public TimeSpan Duration { get; }

		public Service(int id, string name, decimal price, TimeSpan duration)
		{
			Id = id;
			Name = name;
			Price = price;
			Duration = duration;
		}
	}
}
