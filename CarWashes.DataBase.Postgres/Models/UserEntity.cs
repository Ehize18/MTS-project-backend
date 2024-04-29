namespace CarWashes.DataBase.Postgres.Models
{
	public class UserEntity
	{
		public int Id { get; set; }
		public int? HumanId { get; set; }
		public HumanEntity? Human { get; set; }
		public List<OrderEntity> Orders { get; set; } = [];
		public List<CarwashEntity> Carwashes { get; set; } = [];

		public string Role { get; set; } = string.Empty;
		public string? Login { get; set; }
		public string? Password { get; set; }
		public string? Vk_token { get; set; }
	}
}
