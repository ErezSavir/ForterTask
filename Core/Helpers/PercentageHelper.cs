namespace Core.Helpers;

public class PercentageHelper : IPercentageHelper
{
    public decimal GetPercentageIncrease(decimal original, decimal current) =>
        (current - original) / original * 100;
}