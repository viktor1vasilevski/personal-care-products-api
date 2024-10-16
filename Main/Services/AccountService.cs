using Azure;
using Main.Constants;
using Main.DTOs.Account;
using Main.DTOs.Responses;
using Main.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Main.Services;

public class AccountService : IAccountService
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IConfiguration _configuration;
    public AccountService(UserManager<IdentityUser> userManager,
        SignInManager<IdentityUser> signInManager,
        RoleManager<IdentityRole> roleManager,
        IConfiguration configuration)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _roleManager = roleManager;
        _configuration = configuration;
    }

    public async Task<ApiResponse<LoginUserResponse>> Login(LoginUserRequest model)
    {
        try
        {
            var signInResult = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, false, false);
            if (signInResult.Succeeded)
            {
                var user = await _userManager.FindByNameAsync(model.UserName);
                var roles = await _userManager.GetRolesAsync(user);

                IdentityOptions identityOptions = new IdentityOptions();
                var claims = new Claim[]
                {
                    new Claim(identityOptions.ClaimsIdentity.UserIdClaimType, user.Id),
                    new Claim(identityOptions.ClaimsIdentity.UserNameClaimType, user.UserName),
                    new Claim(identityOptions.ClaimsIdentity.RoleClaimType, roles[0])
                };

                var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration["JwtSettings:Secret"]));
                var signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);
                var token = new JwtSecurityToken(
                    signingCredentials: signingCredentials,
                    expires: DateTime.Now.AddHours(1),
                    claims: claims
                );

                var loginUserModel = new LoginUserResponse
                {
                    Token = new JwtSecurityTokenHandler().WriteToken(token),
                    UserId = user.Id,
                    Username = user.UserName,
                    Role = roles[0]
                };

                return new ApiResponse<LoginUserResponse>
                {
                    Data = loginUserModel,
                    Message = AccountConstants.SUCCESSFULLY_LOGGED_IN,
                    Success = true
                };
            }
        }
        catch (Exception ex)
        {
            return new ApiResponse<LoginUserResponse>
            {
                Success = false,
                Message = AccountConstants.ERROR_LOGGING_IN,
                ExceptionMessage = ex.Message
            };
        }


        return new ApiResponse<LoginUserResponse>
        {
            Success = false,
            Message = AccountConstants.INVALID_USER_OR_PASS
        };
    }

    public async Task<ApiResponse<LogoutResponse>> Logout()
    {
        try
        {
            await _signInManager.SignOutAsync();
            return new ApiResponse<LogoutResponse> 
            { 
                Success = true, 
                Data = new LogoutResponse { IsLoggedOut = true }, 
                Message = AccountConstants.SUCCESSFULLY_LOGGED_OUT 
            };
        }
        catch (Exception ex)
        {
            return new ApiResponse<LogoutResponse> 
            { 
                Success = false, 
                Message = AccountConstants.ERROR_LOGGING_OUT, 
                ExceptionMessage = ex.Message 
            };
        }
    }

    public async Task<ApiResponse<IdentityUser>> Register(RegisterUserRequest model)
    {
        try
        {
            var response= new ApiResponse<IdentityUser>();

            IdentityUser user = new IdentityUser()
            {
                UserName = model.UserName,
                Email = model.Email
            };

            IdentityResult userResult = await _userManager.CreateAsync(user, model.Password);

            if (userResult.Succeeded)
            {
                bool roleExcist = await _roleManager.RoleExistsAsync("User");
                if (!roleExcist)
                {
                    await AddRole("User");
                }

                var roleResult = await _userManager.AddToRoleAsync(user, "ss");
                if (roleResult.Succeeded)
                {
                    response.Data = user;
                    response.Success = true;
                    response.Message = AccountConstants.REGISTRATION_WAS_SUCCESS;
                    return response;
                }

                foreach (var error in roleResult.Errors)
                {
                    response.Errors[error.Code] = response.Errors[error.Code].Append(error.Description).ToArray();
                    response.Errors.Add(error.Code, new[] { error.Description });
                }

                return response;
            }

            foreach (var error in userResult.Errors)
            {
                response.Errors[error.Code] = response.Errors[error.Code].Append(error.Description).ToArray();
                response.Errors.Add(error.Code, new[] { error.Description });
            }
            return new ApiResponse<IdentityUser> { Success = false, Message = AccountConstants.REGISTRATION_ERROR, Errors = response.Errors };
        }
        catch (Exception ex)
        {
            return new ApiResponse<IdentityUser> { Success = false, Message = AccountConstants.REGISTRATION_ERROR, ExceptionMessage = ex.Message };
        }
    }

    private async Task<IdentityResult> AddRole(string roleName)
    {
        var role = new IdentityRole();
        role.Name = roleName;
        return await _roleManager.CreateAsync(role);
    }
}
