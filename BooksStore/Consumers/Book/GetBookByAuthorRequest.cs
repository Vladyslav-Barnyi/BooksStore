using BooksStore.Consumers.Base;
using Microsoft.Build.Framework;

namespace BooksStore.Consumers.Book;

public class GetBookByAuthorRequest : PageRequest
{
    [Required] public Guid AuthorId { get; set; }
}