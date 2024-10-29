using Main.DTOs.Account;
using Main.Responses;
using Microsoft.AspNetCore.Identity;

namespace Main.Interfaces;

public interface IAccountService
{
    Task<QueryResponse<LogoutResponse>> Logout();
    Task<QueryResponse<LoginUserResponse>> Login(LoginUserRequest model);
    Task<QueryResponse<IdentityUser>> Register(RegisterUserRequest model);
}
