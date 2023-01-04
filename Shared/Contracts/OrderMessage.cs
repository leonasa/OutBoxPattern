namespace Shared.Contracts;

public record OrderMessage(Order PlacedOrder)
{
    public const string Topic = "order";
}