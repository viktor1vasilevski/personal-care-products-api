using System.ComponentModel.DataAnnotations;

namespace Main.DTOs.Account;

public class LoginUserRequest
{
    [Required]
    public string UserName { get; set; }
    [Required]
    public string Password { get; set; }
}
