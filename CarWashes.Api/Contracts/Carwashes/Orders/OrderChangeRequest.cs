using CarWashes.Core;
using System.ComponentModel.DataAnnotations;

namespace CarWashes.Api.Contracts.Carwashes.Orders
{
	public record OrderChangeRequest(
		int OrderId, [Required] Status Status
		);
}
