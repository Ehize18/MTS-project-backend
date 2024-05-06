namespace CarWashes.DataBase.Postgres.Models
{
	public class CarwashEntity
	{
		public int? Id { get; set; }
		public string OrgName { get; set; } = string.Empty;
		public string Name { get; set; } = string.Empty;
		public string City { get; set; } = string.Empty;
		public string Address { get; set; } = string.Empty;
		public string Phone { get; set; } = string.Empty;
		public string Email { get; set; } = string.Empty;
		public TimeOnly WorkTimeStart { get; set; }
		public TimeOnly WorkTimeEnd { get; set; }

		public List<UserEntity> Staff { get; set; } = [];
		public List<PostEntity> Posts { get; set; } = [];
		public List<ServiceEntity> Services { get; set; } = [];
	}
}
