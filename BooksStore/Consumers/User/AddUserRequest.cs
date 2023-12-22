using System.ComponentModel.DataAnnotations;

namespace BooksStore.Consumers.User;

public class AddUserRequest
{
    [Microsoft.Build.Framework.Required]
    [StringLength(100, ErrorMessage = "Username must be less than 100 characters.")]
    public string UserName { get; set; }

    [Microsoft.Build.Framework.Required]
    [EmailAddress(ErrorMessage = "Invalid Email Address")]
    public string Email { get; set; }

    [Microsoft.Build.Framework.Required]
    [StringLength(100, ErrorMessage = "Password must be between 6 and 100 characters.", MinimumLength = 6)]
    [DataType(DataType.Password)]
    public string Password { get; set; }
}