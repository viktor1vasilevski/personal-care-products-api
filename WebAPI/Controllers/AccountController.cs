using Main.DTOs.Account;
using Main.DTOs.Responses;
using Main.Interfaces;
using Main.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebAPI.Models;

namespace WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
[AllowAnonymous]
public class AccountController : ControllerBase
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IConfiguration _configuration;

    private readonly IAccountService _accountService;

    public AccountController(UserManager<IdentityUser> userManager, 
        SignInManager<IdentityUser> signInManager, 
        RoleManager<IdentityRole> roleManager, 
        IConfiguration configuration, IAccountService accountService)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _roleManager = roleManager;
        _configuration = configuration;
        _accountService = accountService;
    }

    [HttpPost("RegisterUser")]
    //[SwaggerResponse(HttpStatusCode.BadRequest, null, Description = "Registration Form Input Is Not Correct")]
    //[SwaggerResponse(HttpStatusCode.OK, typeof(RegisterViewModel), Description = "Valid Registration")]
    public async Task<QueryResponse<IdentityUser>> Register(RegisterViewModel model)
    {
        try
        {
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

                var roleResult = await _userManager.AddToRoleAsync(user, "User");
                if (roleResult.Succeeded)
                {
                    return new QueryResponse<IdentityUser> { Data = user, Success = true, Message = "User registered" };
                }

                foreach (var error in roleResult.Errors)
                {
                    ModelState.AddModelError(error.Code, error.Description);
                }
            }

            foreach (var error in userResult.Errors)
            {
                ModelState.AddModelError(error.Code, error.Description);
            }
            return new QueryResponse<IdentityUser> { Success = false, Message = "Error registering" };
        }
        catch (Exception ex)
        {
            return new QueryResponse<IdentityUser> { Success = false, Message = "Error registering", ExceptionMessage = ex.Message };
        }
        
    }

    private async Task<IdentityResult> AddRole(string roleName)
    {
        var role = new IdentityRole();
        role.Name = roleName;
        return await _roleManager.CreateAsync(role);
    }

    [HttpPost("LogoutUser")]
    //[SwaggerResponse(HttpStatusCode.NoContent, null, Description = "Valid Sign Out")]
    public IActionResult LogoutUser()
    {
        var response = _accountService.Logout();
        return Ok(response.Result);

    }

    [HttpPost("LoginUser")]
    //[SwaggerResponse(HttpStatusCode.OK, typeof(SignInViewModel), Description = "Valid Sign In")]
    //[SwaggerResponse(HttpStatusCode.BadRequest, null, Description = "User Not Found")]
    public IActionResult LoginUser(LoginUserRequest model)
    {
        var response = _accountService.Login(model);
        return Ok(response.Result);    
    }

}
