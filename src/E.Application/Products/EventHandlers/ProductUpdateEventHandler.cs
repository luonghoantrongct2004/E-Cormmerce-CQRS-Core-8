﻿using E.DAL.UoW;
using E.Domain.Entities.Products.Events;
using E.Domain.Entities.Products;
using MediatR;

namespace E.Application.Products.EventHandlers;

public class ProductUpdateEventHandler: INotificationHandler<ProductCreateAndUpdateEvent>
{
    private readonly IReadUnitOfWork _readUnitOfWork;

    public ProductUpdateEventHandler(IReadUnitOfWork readUnitOfWork)
    {
        _readUnitOfWork = readUnitOfWork;
    }

    public async Task Handle(ProductCreateAndUpdateEvent notification, CancellationToken cancellationToken)
    {
        var product = new Product
        {
            Id = notification.ProductId,
            ProductName = notification.ProductName,
            Description = notification.Description,
            Price = notification.Price,
            Images = notification.Images,
            CategoryId = notification.CategoryId,
            BrandId = notification.BrandId,
            StockQuantity = notification.StockQuantity,
            Discount = notification.Discount
        };
        await _readUnitOfWork.Products.UpdateAsync(product.Id, product);
    }
}