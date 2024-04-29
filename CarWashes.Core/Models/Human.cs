namespace CarWashes.Core.Models
{
	public class Human
	{
		public int? Id { get; }
		public string LastName { get; } = string.Empty;
		public string FirstName { get; } = string.Empty;
		public string? MiddleName { get; } = string.Empty;
		public string Phone { get; } = string.Empty;
		public DateOnly Birthday { get; }
		public string Email { get; } = string.Empty;

		public Human(
			int? id,
			string lastname, string firstname, string middlename,
			string phone,
			DateOnly birthday,
			string email)
		{
			Id = id;
			LastName = lastname;
			FirstName = firstname;
			MiddleName = middlename;
			Phone = phone;
			Birthday = birthday;
			Email = email;
		}
	}
}
