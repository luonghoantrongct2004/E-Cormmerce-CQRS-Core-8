﻿using E.Application.Brands.Commands;
using E.Application.Models;
using E.Application.Services.BrandServices;
using E.DAL.EventPublishers;
using E.DAL.UoW;
using E.Domain.Entities.Brand;
using E.Domain.Entities.Brands.Events;
using MediatR;

namespace E.Application.Brands.CommandHandlers;

public class CreateBrandCommandHandler : IRequestHandler<CreateBrandCommand,
    OperationResult<Brand>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IEventPublisher _eventPublisher;
    private readonly BrandService _brandService;

    public CreateBrandCommandHandler(IUnitOfWork unitOfWork,
        IEventPublisher eventPublisher, BrandService brandService)
    {
        _unitOfWork = unitOfWork;
        _eventPublisher = eventPublisher;
        _brandService = brandService;
    }

    public async Task<OperationResult<Brand>> Handle(CreateBrandCommand request,
        CancellationToken cancellationToken)
    {
        var result = new OperationResult<Brand>();
        try
        {
            await _unitOfWork.BeginTransactionAsync();

            var brand = _brandService.CreateBrand(request.BrandName);

            await _unitOfWork.Brands.AddAsync(brand);
            await _unitOfWork.CompleteAsync();

            var brandCreatedEvent = new BrandCreateEvent(brand.Id, brand.BrandName);
            await _eventPublisher.PublishAsync(brandCreatedEvent);

            await _unitOfWork.CommitAsync();

            result.Payload = brand;
        }
        catch (Exception e)
        {
            await _unitOfWork.RollbackAsync();
            result.AddUnknownError(e.Message);
        }

        return result;
    }
}