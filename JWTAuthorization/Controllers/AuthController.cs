using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
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
		private readonly IConfiguration configuration;

		public AuthController(IConfiguration configuration)
		{
			this.configuration = configuration;
		}

		[HttpGet]
		public async Task<IActionResult> Login()
		{
			var authClaims = new List<Claim>
			{
				new Claim(ClaimTypes.Name, "userName"),
			};

			var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this.configuration["JWT:Secret"]));

			var token = new JwtSecurityToken(
				issuer: this.configuration["JWT:ValidIssuer"],
				audience: this.configuration["JWT:ValidAudience"],
				expires: DateTime.Now.AddHours(3),
				claims: authClaims,
				signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
				);

			return Ok(new LoginResponse
			{
				Token = new JwtSecurityTokenHandler().WriteToken(token),
				Expiration = token.ValidTo
			});
		}
	}
}