using AutoMapper;
using WeatherProject.DTOs;
using WeatherProject.Models;

namespace WeatherProject.AutoMapper
{
    public class Mappings : Profile
    {
        public Mappings()
        {
            CreateMap<Weather, WeatherDto>().ReverseMap();
            CreateMap<User, RegisterUser>().ReverseMap();
        }
    }
}
