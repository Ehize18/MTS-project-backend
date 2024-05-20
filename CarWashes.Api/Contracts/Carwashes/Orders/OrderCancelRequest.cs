namespace CarWashes.Api.Contracts.Carwashes.Orders
{
	public record OrderCancelRequest(
			int Id, string Reason
		);
}
