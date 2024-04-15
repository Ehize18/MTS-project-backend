namespace CarWashes.DataBase.Postgres.Models
{
	public class PostEntity
	{
		public int Id { get; set; }
		public CarwashEntity? CarWash { get; set; }
		public int InternalNumber { get; set; }

		public List<OrderEntity> Orders { get; set; } = [];
	}
}
