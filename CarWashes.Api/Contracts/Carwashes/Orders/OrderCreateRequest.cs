namespace CarWashes.Api.Contracts.Carwashes.Orders
{
	public record OrderCreateRequest(
		int PostId,
		string PlateNumber, string CarBrand, string CarModel, int CarReleaseYear,
		DateTime OrderTime, List<int> ServiceIds);
}
