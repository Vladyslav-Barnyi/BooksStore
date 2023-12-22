using System.ComponentModel.DataAnnotations;

namespace BooksStore.Consumers.Book;

public class ManageBookGenresRequest
{
    [Required] public Guid BookId { get; set; }
    [Required] public bool AddGenres { get; set; }
    
    public List<Guid> GenreIds { get; set; } = [];
}