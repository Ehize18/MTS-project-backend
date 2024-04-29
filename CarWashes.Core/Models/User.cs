namespace CarWashes.Core.Models
{
	public class User
	{
		public int? Id { get; }
		public int? HumanId { get; }
		public string Role { get; } = string.Empty;
		public string? Login { get; } = string.Empty;
		public string? Password { get; } = string.Empty;
		public string? VkToken { get; } = string.Empty;

		public User(
			int? id, int? humanid,
			string role,
			string login, string password,
			string vktoken)
		{
			Id = id;
			HumanId = humanid;
			Role = role;
			Login = login;
			Password = password;
			VkToken = vktoken;
		}
	}
}
