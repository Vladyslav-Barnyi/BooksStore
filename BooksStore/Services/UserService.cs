using BooksStore.Consumers.User;
using BooksStore.Repositories.Interfaces;
using BooksStore.Services.Interfaces;
using BooksStoreEntities.Entities;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace BooksStore.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<List<ApplicationUser>> GetUsersAsync(int page, int pageSize,
        CancellationToken ct = default)
    {
        var users = await _userRepository.GetUsersAsync(page, pageSize, ct);

        return users;
    }

    public async Task<ApplicationUser?> FindAsync(Guid userId,
        CancellationToken ct = default)
    {
        var user = await _userRepository.FindAsync(userId, ct);

        return user;
    }

    public async Task<ApplicationUser> AddAsync(ApplicationUser user,
        CancellationToken ct = default)
    {
        await _userRepository.AddAsync(user, ct);

        return user;
    }

    public Task UpdateAsync(ApplicationUser user,
        CancellationToken ct = default)
    {
        return _userRepository.UpdateAsync(user, ct);
    }

    public Task RemoveAsync(ApplicationUser user,
        CancellationToken ct = default)
    {
        return _userRepository.RemoveAsync(user, ct);
    }

    public Task<bool> CheckModelState(JsonPatchDocument<PatchUserRequest> r, PatchUserRequest patch, 
        ModelStateDictionary modelState)
    {
        r.ApplyTo(patch, modelState);

        return Task.FromResult(modelState.IsValid);
    }
}