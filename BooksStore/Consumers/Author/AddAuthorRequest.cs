using Microsoft.Build.Framework;

namespace BooksStore.Consumers.Author;

public class AddAuthorRequest
{

    [Required] public string FirstName { get; set; }
    [Required] public string LastName { get; set; }
    [Required] public DateTime BirthDate { get; set; }

    public List<Guid> BookIds { get; set; } = [];
}