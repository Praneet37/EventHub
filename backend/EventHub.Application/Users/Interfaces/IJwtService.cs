namespace EventHub.Application.Users.Interfaces;

public interface IJwtService
{
    string GenerateToken(Guid id, string email, string fullName);
}