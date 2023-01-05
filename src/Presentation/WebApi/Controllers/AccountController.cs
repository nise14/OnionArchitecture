using Application.Dtos.Users;
using Application.Features.Authenticate.Commands.AuthenticateCommand;
using Application.Features.Authenticate.Commands.RegisterCommand;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AccountController : BaseApiController
{
    [HttpPost("authenticate")]
    public async Task<IActionResult> AuthenticationAsync(AuthenticationRequest request)
    {
        return Ok(await Mediator.Send(new AuthenticateCommand
        {
            Email = request.Email,
            Password = request.Password,
            IpAddress = GenerateIpAddress()
        }));
    }

    [HttpPost("register")]
    public async Task<IActionResult> RegisterAsync(RegisterRequest request)
    {
        return Ok(await Mediator.Send(new RegisterCommand
        {
            Email = request.Email,
            Password = request.Password,
            ConfirmPassword = request.ConfirmPassword,
            LastName = request.LastName,
            Name = request.Name,
            UserName = request.UserName,
            Origin = Request.Headers["origin"]
        }));
    }

    private string GenerateIpAddress()
    {
        if (Request.Headers.ContainsKey("X-Forwarded-For"))
        {
            return Request.Headers["X-Forwarded-For"];
        }
        else
        {
            return HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
        }
    }
}