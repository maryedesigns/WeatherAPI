using WeatherProject.Models;

namespace WeatherProject.Auth
{
    public interface IAuthManager
    {
        Task<bool> ValidateUser(LoginUser loginUser);
        Task<string> CreateToken(User user);
    }
}
