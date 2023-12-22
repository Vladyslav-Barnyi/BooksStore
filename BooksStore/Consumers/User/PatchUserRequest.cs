namespace BooksStore.Consumers.User;

public class PatchUserRequest
{
    public string? UserName { get; set; }
    public string? Email { get; set; }
    public string? Password { get; set; }

    public void ApplyTo(PatchUserRequest patch, object modelState)
    {
        throw new NotImplementedException();
    }
}