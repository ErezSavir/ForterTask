namespace Core.Validators;

public interface IPerformanceRequestValidator
{
    bool Validate(IEnumerable<string> symbols, DateTimeOffset date);
}