using Main.DTOs.Account;
using Main.DTOs.Responses;
using Main.Responses;
using Microsoft.AspNetCore.Identity;

namespace Main.Interfaces;

public interface IAccountService
{
    Task<ApiResponse<LogoutResponse>> Logout();
    Task<ApiResponse<LoginUserResponse>> Login(LoginUserRequest model);
    Task<ApiResponse<IdentityUser>> Register(RegisterUserRequest model);
}
