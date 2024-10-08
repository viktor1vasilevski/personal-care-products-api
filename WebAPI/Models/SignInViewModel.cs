using System.ComponentModel.DataAnnotations;

namespace WebAPI.Models;

public class SignInViewModel
{
    [Required]
    public string UserName { get; set; }
    [Required]
    public string Password { get; set; }
}
