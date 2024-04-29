namespace CarWashes.DataBase.Postgres.Models
{
	public class HumanEntity
	{
		public int Id { get; set; }
		public List<UserEntity> Users { get; set; } = [];

		public string L_Name { get; set; } = string.Empty;
		public string F_Name { get; set; } = string.Empty;
		public string? M_Name { get; set; }
		public DateOnly Birthday { get; set; }

		public string Phone { get; set; } = string.Empty;
		public string Email { get; set; } = string.Empty;
	}
}
