using System;
using System.Text.Json.Serialization;

namespace JWTAuthorization
{
	public class WeatherForecast
	{
		[JsonPropertyName("date")]
		public DateTime Date { get; set; }

		[JsonPropertyName("summary")]
		public string Summary { get; set; }

		[JsonPropertyName("temperaturec")]
		public int TemperatureC { get; set; }

		[JsonPropertyName("temperaturef")]
		public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
	}
}