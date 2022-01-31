namespace ForterTask.Models;

public class SearchRequest
{
    public IEnumerable<string> Symbols { get; set; } = new List<string>();
    public DateTimeOffset Date { get; set; }
}