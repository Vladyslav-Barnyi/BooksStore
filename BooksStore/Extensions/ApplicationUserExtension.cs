using BooksStore.Consumers.User;
using BooksStoreEntities.Entities;

namespace BooksStore.Extensions;

public static class ApplicationUserExtension
{
    public static ApplicationUser ToUser(this AddUserRequest request)
    {
        var response = new ApplicationUser
        {
            UserName = request.UserName,
            Email = request.Email,
            Password = request.Password
        };

        return response;
    }

    public static GetUserResponse ToUserResponse(this ApplicationUser user)
    {
        var response = new GetUserResponse
        {
            UserName = user.UserName,
            Email = user.Email,
            ShoppingCartId = user.ShoppingCartId,
            Orders = user.Orders.Select(o=>o.ToOrderModel()).ToList()
        };

        return response;
    }

    public static PatchUserRequest ToPatchUserRequest(this ApplicationUser user)
    {
        var response = new PatchUserRequest
        {
            UserName = user.UserName,
            Email = user.Email,
            Password = user.Password
        };

        return response;
    }
    
    public static void Adapt(
        this PatchUserRequest request, ApplicationUser user)
    {
        if (request.UserName is not null)
            user.UserName = request.UserName;
        
        if (request.Email is not null)
            user.Email = request.Email;
        
        if (request.Password is not null)
            user.Password = request.Password;
    }
}