using BooksStoreEntities.Entities;

namespace BooksStore.Repositories.Interfaces;

public interface IUserRepository
{
    Task<List<ApplicationUser>> GetUsersAsync(int page, int pageSize,
        CancellationToken ct = default);

    Task<ApplicationUser?> FindAsync(Guid userId,
        CancellationToken ct = default);

    Task<ApplicationUser> AddAsync(ApplicationUser user,
        CancellationToken ct = default);

    Task UpdateAsync(ApplicationUser user,
        CancellationToken ct = default);

    Task RemoveAsync(ApplicationUser user,
        CancellationToken ct = default);
}