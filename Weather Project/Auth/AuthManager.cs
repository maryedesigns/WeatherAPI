using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WeatherProject.Models;

namespace WeatherProject.Auth
{
    public class AuthManager : IAuthManager

    {
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _configuration;

        public AuthManager(UserManager<User> userManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
        }


        public async Task<string> CreateToken(User user)
        {
         
           var signingCredentials = GetSigningCredentials();
            var claims = await GetClaims(user);
           

            var token = GenerateTokenOptions(signingCredentials, claims);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }


        private JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials, List<Claim> claims) 
        {
            var jwtSettings = _configuration.GetSection("Jwt");

            var token = new JwtSecurityToken(
                 issuer: null,
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: signingCredentials
                );
            return token;
        }


        private async Task<List<Claim>> GetClaims(User _user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, _user.Email),
                new Claim(ClaimTypes.Name, _user.UserName)
            };

            //var roles = await _userManager.GetRolesAsync(_user);
            //foreach (var role in roles)
            //{
            //    claims.Add(new Claim(ClaimTypes.Role, role));
            //}

            return claims;

        }


        private SigningCredentials GetSigningCredentials() 
        {
            var Key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("JWTSettings = TokenKey"));
           
            return new SigningCredentials(Key, SecurityAlgorithms.HmacSha512);
        }
        

        public async Task<bool> ValidateUser(LoginUser loginUser)
        {
            //_user = await _userManager.FindByNameAsync(userDTO.Email);
            //return (_user != null && await _userManager.CheckPasswordAsync(_user, userDTO.Password));

            var storedUser = await _userManager.FindByNameAsync(loginUser.Email);
            return (storedUser != null && await _userManager.CheckPasswordAsync(storedUser, loginUser.Password));
        }
    }
    
}
