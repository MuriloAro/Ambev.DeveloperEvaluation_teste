using System;
using System.Collections.Generic;
using System.Linq;
using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Enums;

namespace Ambev.DeveloperEvaluation.Domain.Entities;

public class Cart : BaseEntity
{
    private readonly List<CartItem> _items;

    public Cart(Guid userId)
    {
        UserId = userId;
        Status = CartStatus.Active;
        _items = new List<CartItem>();
        CreatedAt = DateTime.UtcNow;
    }

    public Guid UserId { get; private set; }
    public IReadOnlyCollection<CartItem> Items => _items.AsReadOnly();
    public CartStatus Status { get; private set; }
    public decimal TotalAmount => _items.Sum(i => i.TotalAmount);
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    public void AddItem(Product product, int quantity)
    {
        var existingItem = _items.FirstOrDefault(i => i.ProductId == product.Id);
        
        if (existingItem != null)
        {
            existingItem.UpdateQuantity(quantity);
        }
        else
        {
            _items.Add(new CartItem(product.Id, quantity, product.Price));
        }

        UpdatedAt = DateTime.UtcNow;
    }

    public void RemoveItem(Guid productId)
    {
        var item = _items.FirstOrDefault(i => i.ProductId == productId);
        if (item != null)
        {
            _items.Remove(item);
            UpdatedAt = DateTime.UtcNow;
        }
    }

    public void Complete()
    {
        Status = CartStatus.Completed;
        UpdatedAt = DateTime.UtcNow;
    }
} 