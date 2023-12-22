using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace BooksStore.DTO;

public class UpdateUserResultDto
{
    public required bool IsValid { get; set; }
    public required ModelStateDictionary ModelState { get; set; }
}