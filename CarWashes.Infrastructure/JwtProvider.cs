using CarWashes.Core.Models;
using CarWashes.Core.Interfaces;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CarWashes.Infrastructure
{
	public class JwtProvider : IJwtProvider
	{
		private readonly JwtOptions _options;

		public JwtProvider(IOptions<JwtOptions> options)
		{
			_options = options.Value;
		}
		public string GenerateToken(User user)
		{
			Claim[] claims = [new("userId", user.Id.ToString()), new("role", user.Role)];
			var signingCredentials = new SigningCredentials(
				new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SecretKey)), SecurityAlgorithms.HmacSha256);
			var token = new JwtSecurityToken(
				claims: claims,
				signingCredentials: signingCredentials,
				expires: DateTime.UtcNow.AddHours(_options.ExpiredHours)
				);

			var tokenValue = new JwtSecurityTokenHandler().WriteToken(token);
			return tokenValue;
		}

		public int GetId(string token)
		{
			var jsonToken = new JwtSecurityTokenHandler().ReadToken(token);
			var decoddedToken = jsonToken as JwtSecurityToken;
			var id = decoddedToken.Claims.First(c => c.Type == "userId").Value;
			return int.Parse(id);
		}
	}
}
