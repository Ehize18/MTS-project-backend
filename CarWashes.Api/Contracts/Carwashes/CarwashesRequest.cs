namespace CarWashes.Api.Contracts.Carwashes
{
	public record CarwashesRequest(
			string orgName, string name,
			string city, string address,
			string phone, string email,
			TimeOnly workTimeStart, TimeOnly workTimeEnd);
}
