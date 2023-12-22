namespace BooksStore.Consumers.Base;

public abstract class PageRequest
{
    public int Page { get; set; } = 1;

    public int PageSize { get; set; } = 10;
}