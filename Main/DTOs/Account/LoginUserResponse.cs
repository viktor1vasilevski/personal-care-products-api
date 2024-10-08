namespace Main.DTOs.Account;

public class LoginUserResponse
{
    public string Token { get; set; }
    public string UserId { get; set; }
    public string Username { get; set; }
    public string Role { get; set; }
}
