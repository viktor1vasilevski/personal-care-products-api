using Main.DTOs.Account;
using Main.DTOs.Responses;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Main.Interfaces;

public interface IAccountService
{
    Task<ApiResponse<BaseResponse>> Logout();
    Task<ApiResponse<LoginUserResponse>> Login(LoginUserRequest model);
    Task<ApiResponse<IdentityUser>> Register(RegisterUserRequest model);
}
