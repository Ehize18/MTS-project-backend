namespace CarWashes.Api.Contracts.Carwashes
{
	public record CarwashesServiceResponse(
		int id, string name, decimal price, TimeSpan duration
		);
}
