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
        // Check if the email already exists
        var existingUser = await spy.GetByEmailAsync(request.Email);

        if (existingUser != null)
        {
            throw new Exception("Email already exists.");
        }

        // Create a new user
        var user = new User
        {
            Id = Guid.NewGuid(),
            FullName = request.FullName,
            Email = request.Email,

            // Temporary: store password directly.
            // We'll replace this with password hashing in a later step.
            PasswordHash = request.Password,

            CreatedAt = DateTime.UtcNow
        };

        // Save the user
        await spy.AddAsync(user);
        await spy.SaveChangesAsync();

        // Return the response
        return new UserResponse
        {
            Id = user.Id,
            FullName = user.FullName,
            Email = user.Email
        };
    }
}