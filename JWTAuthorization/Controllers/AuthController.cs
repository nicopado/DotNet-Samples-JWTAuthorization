using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace JWTAuthorization.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class AuthController : ControllerBase
	{
		[HttpGet]
		public async Task<IActionResult> Login()
		{
			var authClaims = new List<Claim>
			{
				new Claim(ClaimTypes.Name, "userName"),
			};

			var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("my_dirty_long_little_sercret_longer_longer"));

			var token = new JwtSecurityToken(
				issuer: "Issuer",
				audience: "Audience",
				expires: DateTime.Now.AddHours(3),
				claims: authClaims,
				signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
				);

			return Ok(new
			{
				token = new JwtSecurityTokenHandler().WriteToken(token),
				expiration = token.ValidTo
			});
		}
	}
}