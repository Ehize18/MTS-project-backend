using System.ComponentModel.DataAnnotations;

namespace CarWashes.Api.Contracts.Carwashes.Orders
{
	public record AvailableTimesRequest(
		[Required]List<int> ServiceIds, [Required]DateOnly date);
}
