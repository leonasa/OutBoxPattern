namespace Shared.Contracts;

public record Order(int Id, string CustomerId, string ProductId, int Quantity, DateTime PlacedAt);