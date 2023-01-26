using WeatherProject.Models;

namespace WeatherProject.DTOs
{

    public class WeatherDto
    {
        public int Id;
        public int TemperatureC { get; set; }
        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
        public string? City { get; set; }
        public string? Country { get; set; }
    }

    public class TokenDto
    {
        public string Token { get; set; }
        public string Email { get; set; }
    }

}
