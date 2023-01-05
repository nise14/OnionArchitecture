using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Application.Dtos.Users;
using Application.Enums;
using Application.Exceptions;
using Application.Interfaces;
using Application.Wrappers;
using Domain.Settings;
using Identity.Helpers;
using Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Identity.Services;

public class AccountService : IAccountService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly JWTSettings _jwtSettings;
    private readonly IDateTimeService _dateTimeService;

    public AccountService(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, SignInManager<ApplicationUser> signInManager, IOptions<JWTSettings> jWTSettings, IDateTimeService dateTimeService)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _signInManager = signInManager;
        _jwtSettings = jWTSettings.Value;
        _dateTimeService = dateTimeService;
    }

    public async Task<Response<AuthenticationResponse>> AuthenticateAsync(AuthenticationRequest request, string ipAddress)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);

        if (user is null)
        {
            throw new ApiException($"There is not account register with email ({request.Email})");
        }

        var result = await _signInManager.PasswordSignInAsync(user.UserName, request.Password, false, lockoutOnFailure: false);
        if (!result.Succeeded)
        {
            throw new ApiException($"User not valid {request.Email}");
        }

        JwtSecurityToken jwtSecurityToken = await GenerateJwtToken(user);

        AuthenticationResponse response = new AuthenticationResponse();
        response.Id = user.Id;
        response.JWToken = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
        response.Email = user.Email;
        response.UserName = user.UserName;

        var roleList = await _userManager.GetRolesAsync(user).ConfigureAwait(false);
        response.Roles = roleList.ToList();
        response.IsVerified = user.EmailConfirmed;

        var refreshToken = GenerateRefreshToken(ipAddress);

        response.RefreshToken = refreshToken.Token;
        return new Response<AuthenticationResponse>(response, $"User Authenticated {user.UserName}");
    }

    public async Task<Response<string>> RegisterAsync(RegisterRequest request, string origin)
    {
        var userWithSameUserName = await _userManager.FindByNameAsync(request.UserName);

        if (userWithSameUserName is not null)
        {
            throw new ApiException($"The user name {request.UserName} was registered previously yet");
        }

        var user = new ApplicationUser
        {
            Email = request.Email,
            Name = request.Name,
            LastName = request.LastName,
            UserName = request.UserName,
            EmailConfirmed = true,
            PhoneNumberConfirmed = true
        };

        var userWithSameEmail = await _userManager.FindByEmailAsync(request.Email);

        if (userWithSameEmail is not null)
        {
            throw new ApiException($"The email {request.UserName} was registered previously yet");
        }

        var result = await _userManager.CreateAsync(user, request.Password);

        if (!result.Succeeded)
        {
            throw new ApiException($"{result.Errors}.");
        }

        await _userManager.AddToRoleAsync(user, Roles.Basic.ToString());
        return new Response<string>(user.Id, message: $"Register sucess {request.UserName}");
    }

    private async Task<JwtSecurityToken> GenerateJwtToken(ApplicationUser user)
    {
        var userClaims = await _userManager.GetClaimsAsync(user);
        var roles = await _userManager.GetRolesAsync(user);

        var roleClaims = new List<Claim>();

        for (var i = 0; i < roles.Count; i++)
        {
            roleClaims.Add(new Claim("Roles", roles[i]));
        }

        string ipAddress = IpHelper.GetIpAddress();

        var claims = new[]{
            new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim("uid", user.Id),
            new Claim("ip", ipAddress),
        }
        .Union(userClaims)
        .Union(roleClaims);

        var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
        var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

        var jwtSecurityToken = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            claims: claims,
            expires: DateTime.Now.AddMinutes(_jwtSettings.DurationInMinutes),
            signingCredentials: signingCredentials
        );

        return jwtSecurityToken;
    }

    private RefreshToken GenerateRefreshToken(string ipAddress)
    {
        return new RefreshToken
        {
            Token = RandomTokenString(),
            Expires = DateTime.Now.AddDays(7),
            Created = DateTime.Now,
            CreatedByIp = ipAddress
        };
    }

    private string RandomTokenString()
    {
        using var rngCryptoServiceProvider = new RNGCryptoServiceProvider();
        var randomBytes = new byte[40];
        rngCryptoServiceProvider.GetBytes(randomBytes);
        return BitConverter.ToString(randomBytes).Replace("-", "");
    }
}