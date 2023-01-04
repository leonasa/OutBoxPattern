namespace Shared.Contracts;

public class OrderMessage
{
    public const string Topic = "order";
    public Order PlacedOrder { get; set; }
}