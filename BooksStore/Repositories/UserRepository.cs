using BooksStore.Repositories.Interfaces;
using BooksStoreEntities;
using BooksStoreEntities.Entities;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace BooksStore.Repositories;

public class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext _dbContext;

    public UserRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<ApplicationUser>> GetUsersAsync(int page, int pageSize, CancellationToken ct = default)
    {
        var users = await _dbContext.Users
            .Include(u=>u.Orders)
            .Skip((page - 1) * pageSize)
            .Take(pageSize).ToListAsync(ct);

        return users;
    }

    public async Task<ApplicationUser?> FindAsync(Guid userId, CancellationToken ct = default)
    {
        var user = await _dbContext.Users
            .Where(u => u.Id == userId).FirstOrDefaultAsync(ct);

        return user;
    }

    public async Task<ApplicationUser> AddAsync(ApplicationUser user, CancellationToken ct = default)
    {
        try
        {
            await _dbContext.AddAsync(user, ct);
            await _dbContext.SaveChangesAsync(ct);
        }
        catch (Exception e)
        {
            Log.Error(e, "Could not create user");
            throw;
        }

        return user;
    }

    public async Task UpdateAsync(ApplicationUser user, CancellationToken ct = default)
    {
        _dbContext.Update(user);
        await _dbContext.SaveChangesAsync(ct);
    }

    public async Task RemoveAsync(ApplicationUser user, CancellationToken ct = default)
    {
        _dbContext.Remove(user);
        await _dbContext.SaveChangesAsync(ct);
    }
}