namespace WeatherProject.Models
{
    public class Weather
    {
        public int Id { get; set; }
        public int TemperatureC { get; set; }
        public int TemperatureF { get; set; }   

        public string? City { get; set; }    
        public string? Country { get; set; }
        
        public string? Summary { get; set; }
               
        public DateTime Date { get; set; } = DateTime.Now;
    }  
}