using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using WeatherProject.Data;
using WeatherProject.DTOs;
using WeatherProject.Models;

namespace WeatherProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class WeatherController : ControllerBase
    {
        private readonly WeatherDbContext _context;
        private readonly ILogger<WeatherController> _logger;
        private readonly IMapper _mapper;

        public WeatherController(WeatherDbContext context, ILogger<WeatherController> logger, IMapper mapper)
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
        }

        public string[] Summary = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };


        [HttpGet]
        public async Task<IEnumerable<Weather>> GetAll()
        {

            return await _context.Weathers.ToListAsync();

        }

       // [Authorize]
        [HttpGet("id")]
        public async Task<IActionResult> GetById(int id)
        {
            var weather = await _context.Weathers.FindAsync(id);
            return weather == null ? NotFound() : Ok(weather);
        }

        //[Authorize]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] WeatherDto weatherDto)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var weather = _mapper.Map<Weather>(weatherDto);
           
            var cast = Summary[Random.Shared.Next(Summary.Length)];
            weather.Summary = cast;
            await _context.Weathers.AddAsync(weather);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = weather.Id }, weather);
        }

        //[Authorize]
        [HttpPut("id")]
        public async Task<IActionResult> Update(int id, Weather weather)
        {
            
            if (id != weather.Id) return BadRequest();
            
            _context.Entry(weather).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        //[Authorize]
        [HttpDelete("id")]
        public async Task<IActionResult> Delete(int id)
        {
            var weatherToDelete = await _context.Weathers.FindAsync(id);    
            if(weatherToDelete == null) return NotFound();

            _context.Weathers.Remove(weatherToDelete);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
