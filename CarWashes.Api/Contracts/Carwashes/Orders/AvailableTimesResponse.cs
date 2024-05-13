namespace CarWashes.Api.Contracts.Carwashes.Orders
{
	public record AvailableTimesResponse(
		List<DateTime> AvailableTimes);
}
