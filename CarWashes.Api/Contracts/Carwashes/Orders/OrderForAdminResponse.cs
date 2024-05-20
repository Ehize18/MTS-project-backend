using CarWashes.Core;

namespace CarWashes.Api.Contracts.Carwashes.Orders
{
	public record OrderForAdminResponse(
		int Id, string FirstName, string Phone,
		int PostInternalNumber,
		string PlateNumber, string CarBrand, string CarModel, int CarReleaseYear,
		Status Status, DateTime OrderTime, DateTime CreatedAt, DateTime UpdatedAt);
}
