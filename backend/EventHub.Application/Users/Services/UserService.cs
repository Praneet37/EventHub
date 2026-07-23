using EventHub.Application.Users.DTOs;
using EventHub.Application.Users.Interfaces;
using EventHub.Domain.Entities;
using EventHub.Domain.Interfaces;

namespace EventHub.Application.Users.Services;

public class UserService : IUserService
{
    private readonly IUserRepository spy;

    public UserService(IUserRepository repository)
    {
        spy = repository;
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
            PasswordHash = request.Password,
            CreatedAt = DateTime.UtcNow
        };

        await spy.AddAsync(user);
        await spy.SaveChangesAsync();

        return new UserResponse
        {
            Id = user.Id,
            FullName = user.FullName,
            Email = user.Email
        };
    }
}