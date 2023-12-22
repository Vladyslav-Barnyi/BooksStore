using BooksStore.Consumers.Base;
using Microsoft.Build.Framework;

namespace BooksStore.Consumers.Book;

public class GetBookByGenreRequest : PageRequest
{
    [Required] public Guid GenreId { get; set; }
}