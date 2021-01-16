using System;
using System.Text.Json.Serialization;

namespace JWTAuthorization
{
	public class LoginResponse
	{
		[JsonPropertyName("expiration")]
		public DateTime Expiration { get; set; }

		[JsonPropertyName("token")]
		public string Token { get; set; }
	}
}