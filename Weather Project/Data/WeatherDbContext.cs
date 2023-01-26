using Microsoft.EntityFrameworkCore;
using WeatherProject.Models;

namespace WeatherProject.Data
{
    public class WeatherDbContext : DbContext
    {
        public WeatherDbContext(DbContextOptions<WeatherDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }   
        public DbSet<Weather> Weathers { get; set; }

    }
}
