namespace Core.Validators;

public class PerformanceRequestValidator : IPerformanceRequestValidator
{
    public bool Validate(IEnumerable<string> symbols, DateTimeOffset date) => 
        symbols.Any() && date <= DateTimeOffset.Now;
}