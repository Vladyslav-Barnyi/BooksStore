using System.ComponentModel.DataAnnotations;

namespace BooksStore.Consumers.Book;

public class ManageBookAuthorsRequest
{
    [Required] public Guid BookId { get; set; }
    [Required] public bool AddAuthors { get; set; }
    
    public List<Guid> AuthorIds { get; set; } = [];
}