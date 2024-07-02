﻿using E.Application.Carts.Commands;
using E.Application.Carts.Events;
using E.Application.Enums;
using E.Application.Identity;
using E.Application.Models;
using E.Application.Products;
using E.DAL.EventPublishers;
using E.DAL.UoW;
using E.Domain.Entities.Carts;
using MediatR;

namespace E.Application.Carts.CommandHandlers;

public class CartItemAddCommandHandler : IRequestHandler<CartItemAddCommand,
    OperationResult<CartDetails>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IEventPublisher _eventPublisher;

    public CartItemAddCommandHandler(IUnitOfWork unitOfWork, IEventPublisher eventPublisher)
    {
        _unitOfWork = unitOfWork;
        _eventPublisher = eventPublisher;
    }

    public async Task<OperationResult<CartDetails>> Handle(CartItemAddCommand request,
        CancellationToken cancellationToken)
    {
        var result = new OperationResult<CartDetails>();
        try
        {
            await _unitOfWork.BeginTransactionAsync();

            var user = await _unitOfWork.Users.GetByIdAsync(request.UserId);
            if (user is null)
            {
                result.AddError(ErrorCode.NotFound,
                    UserErrorMessage.UserNotFound);
                return result;
            }

            var product = await _unitOfWork.Products.GetByIdAsync(request.ProductId);
            if (product is null)
            {
                result.AddError(ErrorCode.NotFound,
                    ProductErrorMessage.ProductNotFound);
                return result;
            }
            if(product.StockQuantity > request.Quantity)
            {
                result.AddError(ErrorCode.ValidationError,
                    CartErrorMessage.ExcessProductInventory);
                return result;
            }
            var existingCartItem = await _unitOfWork.Carts.FirstOrDefaultAsync(cd =>
            cd.UserId == request.UserId && cd.ProductId == request.ProductId);

            if (existingCartItem != null)
            {
                existingCartItem.Quantity += request.Quantity;
                _unitOfWork.Carts.Update(existingCartItem);

                var cartUpdateItemEvent = new CartItemUpdateEvent(existingCartItem.Id,existingCartItem.UserId,
                    existingCartItem.ProductId, existingCartItem.Quantity);
                await _eventPublisher.PublishAsync(cartUpdateItemEvent);

                await _unitOfWork.CommitAsync();

                result.Payload = existingCartItem;
            }
            else
            {
                var cart = CartDetails.AddProductIntoCart(request.UserId,
                    request.ProductId, request.Quantity);
                await _unitOfWork.Carts.AddAsync(cart);

                var cartAddItemEvent = new CartItemAddEvent(cart.Id,
                    cart.UserId, cart.ProductId, cart.Quantity);
                await _eventPublisher.PublishAsync(cartAddItemEvent);

                await _unitOfWork.CommitAsync();

                result.Payload = cart;
            }
        }
        catch (Exception e)
        {
            await _unitOfWork.RollbackAsync();
            result.AddUnknownError(e.Message);
        }

        return result;
    }
}