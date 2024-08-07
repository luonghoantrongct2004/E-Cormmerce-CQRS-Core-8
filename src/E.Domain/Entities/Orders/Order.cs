﻿using E.Domain.Entities.Users;
using E.Domain.Enum;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace E.Domain.Entities.Orders;

public class Order : BaseEntity
{
    [BsonRepresentation(BsonType.String)]
    public Guid UserId { get; set; }

    public DateTime OrderDate { get; set; }

    public DateTime PaymentDate { get; set; }

    public string? Note { get; set; }

    public virtual DomainUser User { get; set; }
    public string PaymentMethod { get; set; }
    public string ContactPhone { get; set; }

    [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
    public decimal TotalPrice { get; set; }

    public ICollection<OrderDetail> OrderDetails { get; set; }

    [NotMapped]
    public OrderStatus Status { get; set; }

    [Column("Status")]
    public string StatusString
    {
        get => Status.ToString();
        set => Status = System.Enum.TryParse(value, out OrderStatus status)
            ? status : OrderStatus.Pending;
    }
}