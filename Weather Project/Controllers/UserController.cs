using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WeatherProject.Auth;
using WeatherProject.DTOs;
using WeatherProject.Models;

namespace WeatherProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly ILogger<UserController> _logger;
        private readonly IMapper _mapper;
        private readonly IAuthManager _authManager;

        public UserController(UserManager<User> userManager,
                SignInManager<User> signInManager,
                ILogger<UserController> logger,
                IMapper mapper,
                IAuthManager authManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _mapper = mapper;
            _authManager = authManager;
        }

        [HttpPost("RegisterUser")]
        public async Task<IActionResult> Register([FromBody] RegisterUser registerUser)
        {
            _logger.LogInformation($"Registration attempt for {registerUser.Email}");
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var user = _mapper.Map<User>(registerUser);
                user.UserName = registerUser.Email;
                var result = await _userManager.CreateAsync(user, registerUser.Password);

                if (!result.Succeeded)
                {
                    return BadRequest($"User Registration Attempt Failed");
                }

                return Accepted();
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Something Went Wrong in the {nameof(Register)}");
                return Problem($"Something Went wrong in the {nameof(Register)}", statusCode: 500);
            }
        }

        [HttpPost("LoginUser")]
        public async Task<ActionResult<TokenDto>> Login([FromBody] LoginUser loginUser)
        {
            _logger.LogInformation($"Login attempt for {loginUser.Email}");
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var user = await _userManager.FindByEmailAsync(loginUser.Email);
                if (user == null || !await _userManager.CheckPasswordAsync(user, loginUser.Password))
                    return Unauthorized();

                return new TokenDto
                {
                    Email = user.Email,
                    Token = await _authManager.CreateToken(user)
                };
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Something Went Wrong in the {nameof(Login)}");
                return Problem($"Something Went wrong in the {nameof(Login)}", statusCode: 500);
            }
        }
    }
}
