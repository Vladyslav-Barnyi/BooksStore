using System.ComponentModel.DataAnnotations.Schema;

namespace BooksStoreEntities.Entities;

public class Genre : BaseEntity
{
    [Column(TypeName = "NVARCHAR(44)")]
    public required string Name { get; set; }
    
    public List<Book> Books { get; set; } = [];
}