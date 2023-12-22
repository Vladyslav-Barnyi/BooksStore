namespace BooksStore.Consumers.Book;

public class PatchBookRequest
{
    public string? Title { get; set; }
    public DateTime? PublicationDate { get; set; }
    public decimal? Price { get; set; }
    public int? ISBN { get; set; }
}