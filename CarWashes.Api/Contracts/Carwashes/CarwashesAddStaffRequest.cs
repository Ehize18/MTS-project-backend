namespace CarWashes.Api.Contracts.Carwashes
{
	public record CarwashesAddStaffRequest(
		int carwashId, string staffPhone);
}
