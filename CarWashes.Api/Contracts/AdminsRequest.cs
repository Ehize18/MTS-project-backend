using System.ComponentModel.DataAnnotations;

namespace CarWashes.Api.Contracts
{
	public record AdminsRequest(
		[Required] string login, [Required] string password,
		string l_name, string f_name, string m_name,
		[Required][MaxLength(11)] string phone, DateOnly birthday,
		string email, bool haveClientAccoint
		);
}