using EventHub.Application.Users.DTOs;
namespace EventHub.Application.Users.Interfaces;

public interface IUserService
{
    Task<UserResponse> RegisterAsync(RegisterUserRequest request);
}