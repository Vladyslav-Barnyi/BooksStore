using BooksStore.Consumers.Base;
using Microsoft.Build.Framework;

namespace BooksStore.Consumers.Order;

public class GetOrdersRequest : PageRequest
{
    public string? Title { get; set; }
    public Guid? AuthorId { get; set; }
    public int? ISBN { get; set; }
    [Required] public FilterType FilterType { get; set; }

    public bool IsValid()
    {
        int filterCount = 0;
        if (FilterType == FilterType.All) filterCount++;
        if (!string.IsNullOrWhiteSpace(Title)) filterCount++;
        if (AuthorId.HasValue) filterCount++;
        if (ISBN.HasValue) filterCount++;

        return filterCount == 1;
    }
}

public enum FilterType
{
    Title,
    Author,
    ISBN,
    All
}