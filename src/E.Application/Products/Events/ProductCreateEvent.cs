﻿using MediatR;
using System.ComponentModel.DataAnnotations;

namespace E.Domain.Entities.Products.Events;

public class ProductCreateEvent : BaseEntity, INotification
{
    public string ProductName { get; set; }

    public string? Description { get; set; }

    [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
    public int Price { get; set; }

    public List<string>? Images { get; set; }

    public Guid CategoryId { get; set; }

    public Guid BrandId { get; set; }
    public Guid? CommentId { get; set; }

    public int StockQuantity { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;

    [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
    public int Discount { get; set; }

    public ProductCreateEvent(Guid productId, string productName, string description, int price,
        List<string> images, Guid categoryId, Guid brandId, int stockQuantity, int discount, DateTime createAt)
    {
        Id = productId;
        ProductName = productName;
        Description = description;
        Price = price;
        Images = images;
        CategoryId = categoryId;
        BrandId = brandId;
        StockQuantity = stockQuantity;
        Discount = discount;
        CreatedAt = createAt;
    }
}