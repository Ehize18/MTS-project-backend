using CarWashes.Core;

namespace CarWashes.Api.Contracts.Carwashes.Orders
{
	public record OrderForClientResponse(
		int Id,
		string PlateNumber, string CarBrand, string CarModel, int CarReleaseYear,
		Status Status, DateTime OrderTime, DateTime UpdatedAt);
}
