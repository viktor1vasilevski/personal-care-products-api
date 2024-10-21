﻿using Data.Context;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Data.Extensions;

public static class IdentityExtension
{
    public static void AddIdentityInfrastructure(this IServiceCollection services)
    {
        services.AddIdentity<IdentityUser, IdentityRole>()
            .AddEntityFrameworkStores<LibraryDbContext>()
            .AddDefaultTokenProviders();

        var singingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("this-is-my-secret-key"));
        var tokenValidationParameters = new TokenValidationParameters()
        {
            IssuerSigningKey = singingKey,
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };
        services.AddAuthentication(x => x.DefaultAuthenticateScheme = JwtBearerDefaults
                .AuthenticationScheme)
                .AddJwtBearer(jwt =>
                {
                    jwt.TokenValidationParameters = tokenValidationParameters;
                });
    }
}