namespace CarWashes.DataBase.Postgres.Models
{
	public class ServiceEntity
	{
		public int? Id { get; set; }
		public int CarwashId { get; set; }
		public CarwashEntity? Carwash { get; set; }
		public string Name { get; set; } = string.Empty;
		public decimal Price { get; set; }
		public TimeSpan Duration { get; set; }

		public List<OrderEntity> Orders { get; set; } = [];
	}
}
