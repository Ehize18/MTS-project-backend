namespace CarWashes.Api.Contracts
{
	public record HumanResponse(
		string LastName, string FirstName, string MiddleName,
		DateOnly Birthday, string Phone, string Email);
}
