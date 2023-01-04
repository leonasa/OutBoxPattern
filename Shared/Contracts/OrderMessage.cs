namespace OutBoxPattern.Services;

public class OrderMessage
{
    public string Topic = "order";
    public Order PlacedOrder { get; set; }
}