using System.Data;
using System.IdentityModel.Tokens.Jwt;

namespace WebAPI.Models
{
    public class LoginUserViewModel
    {
        public string Token { get; set; }
        public string UserId { get; set; }
        public string Username { get; set; }
        public string Role { get; set; }
    }
}