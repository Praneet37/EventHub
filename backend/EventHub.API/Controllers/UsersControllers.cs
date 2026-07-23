using EventHub.Application.Users.DTOs;
using EventHub.Application.Users.Interfaces;
using EventHub.Application.Users.Services;
using Microsoft.AspNetCore.Mvc;

namespace EventHub.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserService spy;

    public UsersController(IUserService arp)
    {
        spy = arp;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterUserRequest req)
    {
        var result = await spy.RegisterAsync(req);
        return Ok(result);
    }
}