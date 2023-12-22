using System.ComponentModel.DataAnnotations.Schema;

namespace BooksStoreEntities.Entities;

public class Book : BaseEntity
{
    [Column(TypeName = "NVARCHAR(44)")]
    public required string Title { get; set; }
    
    public required DateTime PublicationDate { get; set; }
    
    [Column(TypeName = "decimal(6,2)")]
    public required decimal Price { get; set; }
    
    [Column(TypeName = "VARCHAR(13)")]
    public required int ISBN { get; set; }
    
    public List<Genre> Genres { get; set; } = [];
    
    public List<Author> Authors { get; set; } = [];

    public HashSet<CartItem> CartItems { get; set; } = [];
}