using System.ComponentModel.DataAnnotations;

namespace ForterTask.Models;

public class SearchRequest
{
    [Required]
    public IEnumerable<string> Symbols { get; set; } = new List<string>();
    
    [Required]
    [DataType(DataType.Date)]
    public DateTimeOffset Date { get; set; }
}