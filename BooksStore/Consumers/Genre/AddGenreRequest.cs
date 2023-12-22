using Microsoft.Build.Framework;

namespace BooksStore.Consumers.Genre;

public class AddGenreRequest
{
    [Required] public string Name { get; set; }
    
    public List<Guid> BookIds { get; set; } = [];
}