namespace EventHub.Application.Users.DTOs;

public class RegisterUserRequest
{
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string password { get; set; } = string.Empty;
}