﻿using E.Application.Models;
using E.Domain.Entities.Brand;
using MediatR;

namespace E.Application.Brands.Commands;

public class UpdateBrandCommand : IRequest<OperationResult<Brand>>
{
    public Guid Id { get; set; }
    public string BrandName { get; set; }
}