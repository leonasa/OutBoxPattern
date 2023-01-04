namespace Shared.Contracts;

public record OrderFulfilledMessage(Order PlacedOrder)
{
    public const string Topic = "order";
};