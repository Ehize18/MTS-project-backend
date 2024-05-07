namespace CarWashes.Api.Contracts.Carwashes
{
	public record CarwashesResponse(
		int id,
		string orgName, string name,
		string city, string address,
		string phone, string email,
		TimeOnly workTimeStart, TimeOnly workTimeEnd);
}
