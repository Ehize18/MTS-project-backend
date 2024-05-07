namespace CarWashes.Api.Contracts.Carwashes
{
	public record CarwashesAddServiceRequest(
		string name, decimal price, TimeSpan duration
		);
}
