using BooksStore.Consumers.User;
using BooksStoreEntities.Entities;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace BooksStore.Services.Interfaces;

public interface IUserService
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

    public Task<bool> CheckModelState(JsonPatchDocument<PatchUserRequest> r, PatchUserRequest patch, 
        ModelStateDictionary modelState);

}