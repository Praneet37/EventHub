using EventHub.Application.Users.DTOs;
using EventHub.Application.Users.Interfaces;
using EventHub.Domain.Entities;
using EventHub.Domain.Interfaces;

namespace EventHub.Application.Users.Services;

public class UserService : IUserService
{
    private readonly IUserRepository spy;
    private readonly IJwtService jwtService;

    public UserService(IUserRepository repository, IJwtService jwtService)
    {
        spy = repository;
        this.jwtService = jwtService;
    }

    public async Task<UserResponse> RegisterAsync(RegisterUserRequest request)
    {
        var existingUser = await spy.GetByEmailAsync(request.Email);

        if (existingUser != null)
        {
            throw new Exception("Email already exists.");
        }

        var user = new User
        {
            Id = Guid.NewGuid(),
            FullName = request.FullName,
            Email = request.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
            CreatedAt = DateTime.UtcNow
        };

        await spy.AddAsync(user);
        await spy.SaveChangesAsync();

        return new UserResponse
        {
            Id = user.Id,
            FullName = user.FullName,
            Email = user.Email,
            Token = string.Empty
        };
    }

    public async Task<UserResponse> LoginAsync(LoginUserRequest request)
    {
        var user = await spy.GetByEmailAsync(request.Email);

        if (user == null)
        {
            throw new Exception("Invalid email or password.");
        }

        bool isPasswordCorrect = BCrypt.Net.BCrypt.Verify(
            request.Password,
            user.PasswordHash);

        if (!isPasswordCorrect)
        {
            throw new Exception("Invalid email or password.");
        }

        var token = jwtService.GenerateToken(
            user.Id,
            user.Email,
            user.FullName);

        return new UserResponse
        {
            Id = user.Id,
            FullName = user.FullName,
            Email = user.Email,
            Token = token
        };
    }
}