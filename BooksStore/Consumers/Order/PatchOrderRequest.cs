using Microsoft.Build.Framework;

namespace BooksStore.Consumers.Order;

public class PatchOrderRequest
{
    [Required] public required Guid OrderId { get; set; }
    [Required] public required bool AddItems { get; set; }

    public List<Guid> ItemIds { get; set; } = [];
}