using Microsoft.EntityFrameworkCore;

namespace EventHub.Infrastructure.Persistence;

public class  ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }
}