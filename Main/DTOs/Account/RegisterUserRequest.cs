using System.ComponentModel.DataAnnotations;

namespace Main.DTOs.Account;

public class RegisterUserRequest
{
    [Required, RegularExpression(@"^[a-zA-Z0-9_-]{3,15}$")]
    public string UserName { get; set; }

    [Required, EmailAddress]
    public string Email { get; set; }

    [Required, RegularExpression(@"^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$ %^&*-]).{8,}$")]
    public string Password { get; set; }

    [Required, Compare("Password")]
    public string ConfirmPassword { get; set; }
}
