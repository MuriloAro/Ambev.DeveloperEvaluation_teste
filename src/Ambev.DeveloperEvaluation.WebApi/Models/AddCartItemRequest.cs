namespace Ambev.DeveloperEvaluation.WebApi.Models;

public class AddCartItemRequest
{
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
} 