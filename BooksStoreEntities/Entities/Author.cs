using System.ComponentModel.DataAnnotations.Schema;

namespace BooksStoreEntities.Entities;

public class Author : BaseEntity
{
    [Column(TypeName = "NVARCHAR(77)")]
    public required string FirstName { get; set; }
    
    [Column(TypeName = "NVARCHAR(171)")]
    public required string LastName { get; set; }
    
    public required DateTime BirthDate { get; set; }

    public List<Book> Books { get; set; } = [];
}