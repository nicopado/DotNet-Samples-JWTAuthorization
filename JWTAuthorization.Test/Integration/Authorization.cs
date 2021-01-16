using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace JWTAuthorization.Test
{
	public class Authorization
	{
		private readonly HttpClient _client;
		private readonly TestServer _server;

		public Authorization()
		{
			// Arrange

			var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

			_server = new TestServer(new WebHostBuilder().UseConfiguration(configuration).UseStartup<Startup>());

			_client = _server.CreateClient();
		}

		[Fact]
		public async Task ReturnData()
		{
			// Arrange
			var loginResponse = await _client.GetAsync("/Auth");
			loginResponse.EnsureSuccessStatusCode();
			var loginResponseString = await loginResponse.Content.ReadAsStringAsync();
			var token = JsonSerializer.Deserialize<LoginResponse>(loginResponseString).Token;

			var request = new HttpRequestMessage(HttpMethod.Get, "/WeatherForecast");
			request.Headers.Add("Authorization", $"Bearer {token}");

			// Act
			var response = await _client.SendAsync(request);
			response.EnsureSuccessStatusCode();
			var responseString = await response.Content.ReadAsStringAsync();
			var data = JsonSerializer.Deserialize<List<WeatherForecast>>(responseString);

			// Assert
			Assert.NotNull(data);
		}

		[Fact]
		public async Task ReturnToken()
		{
			// Act
			var response = await _client.GetAsync("/Auth");
			response.EnsureSuccessStatusCode();
			var responseString = await response.Content.ReadAsStringAsync();
			var token = JsonSerializer.Deserialize<LoginResponse>(responseString).Token;

			// Assert
			Assert.False(string.IsNullOrEmpty(token));
		}

		[Fact]
		public async Task ReturnUnauthorized()
		{
			// Act
			var response = await _client.GetAsync("/WeatherForecast");

			// Assert
			Assert.Equal(System.Net.HttpStatusCode.Unauthorized, response.StatusCode);
		}
	}
}