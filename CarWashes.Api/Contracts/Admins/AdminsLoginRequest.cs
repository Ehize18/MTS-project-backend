using System.ComponentModel.DataAnnotations;

namespace CarWashes.Api.Contracts.Admins
{
	public record AdminsLoginRequest(
		[Required] string login, [Required] string password);
}
