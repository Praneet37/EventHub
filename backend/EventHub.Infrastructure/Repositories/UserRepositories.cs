using EventHub.Domain.Entities;
using EventHub.Domain.Interfaces;
using EventHub.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace EventHub.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext spy;

    public UserRepository(ApplicationDbContext context)
    {
        spy = context ;
    }
    public async Task <User?> GetByEmailAsync(string email)
    {
        return await spy.Users.FirstOrDefaultAsync(u => u.Email == email);
    }
    
    public async Task<User?> GetByIdAsync(Guid id)
    {
        return await spy.Users.FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task AddAsync (User user)
    {
        await spy.Users.AddAsync(user);
    }
    public async Task SaveChangesAsync()
    {
        await spy.SaveChangesAsync();
    }
}