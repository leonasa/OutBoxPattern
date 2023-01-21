namespace Shared.Contracts;

public record OrderFulfilledMessage(Order PlacedOrder, DateTime FulfilledAt)
{
    public const string Topic = "orderFulfilled";
}