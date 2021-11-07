﻿using MediatR;
using RefactorThis.Application.DTOs;
using RefactorThis.Domain.Entities;
using RefactorThis.Domain.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace RefactorThis.Application
{
    public class UpdateProductRequest : IRequest<Product>
    {
        public ProductDto Product { get; set; }

        public class Handler : IRequestHandler<UpdateProductRequest, Product>
        {
            private readonly IProductRepository _productRepository;

            public Handler(IProductRepository productRepository)
            {
                _productRepository = productRepository;
            }

            public Task<Product> Handle(UpdateProductRequest request, CancellationToken cancellationToken)
            {
                var product = ProductDtoMapper.FromDto(request.Product);

                return _productRepository.UpdateProductAsync(product);
            }
        }
    }
}