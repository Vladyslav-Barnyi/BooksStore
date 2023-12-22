using Microsoft.Build.Framework;

namespace BooksStore.Consumers.Book;

public class AddBookRequest
{
    [Required] public string Title { get; set; }
    [Required] public DateTime PublicationDate { get; set; }
    [Required] public decimal Price { get; set; }
    [Required] public int ISBN { get; set; }
    
    public List<Guid> GenreIds { get; set; } = [];
    public List<Guid> AuthorIds { get; set; } = [];
}