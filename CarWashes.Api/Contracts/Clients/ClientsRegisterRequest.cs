using System.ComponentModel.DataAnnotations;

namespace CarWashes.Api.Contracts.Clients
{
    public record ClientsRegisterRequest(
        [Required] string login, [Required] string password,
        [Required] string l_name, [Required] string f_name, string m_name,
        [Required][MaxLength(11)] string phone, [Required] DateOnly birthday,
        [Required] string email
        );
}
