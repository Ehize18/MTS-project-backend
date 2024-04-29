using System.ComponentModel.DataAnnotations;

namespace CarWashes.Api.Contracts.Clients
{
	public record ClientsLoginRequest(
		[Required]string login, [Required]string password);
}
