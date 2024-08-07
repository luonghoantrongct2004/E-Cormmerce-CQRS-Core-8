﻿using System.ComponentModel.DataAnnotations;

namespace E.API.Contracts.Products.Responses;

public class ProductResponse : BaseEntity
{
    public string ProductName { get; set; }

    public string? Description { get; set; }

    [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
    public int Price { get; set; }

    public List<string>? Images { get; set; }

    public Guid? CategoryId { get; set; }

    public Guid? BrandId { get; set; }
    public Guid? CommentId { get; set; }

    public int StockQuantity { get; set; }
    public int? SoldQuantity { get; set; } = 0;

    public DateTime? CreatedAt { get; set; } = DateTime.Now;

    [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
    public int? Discount { get; set; }

    public Category? Category { get; set; }
    public E.Domain.Entities.Brand.Brand? Brand { get; set; }
    public IEnumerable<E.Domain.Entities.Comment.Comment>? Comments { get; set; }
    public IEnumerable<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
}