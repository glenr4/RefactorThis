using MediatR;
using RefactorThis.Domain.Entities;
using RefactorThis.Domain.Exceptions;
using RefactorThis.Domain.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace RefactorThis.Application
{
    public class GetProductRequest : IRequest<Product>
    {
        public Guid Id { get; set; }

        public class Handler : IRequestHandler<GetProductRequest, Product>
        {
            private readonly IProductRepository _productRepository;

            public Handler(IProductRepository productRepository)
            {
                _productRepository = productRepository;
            }

            public async Task<Product> Handle(GetProductRequest request, CancellationToken cancellationToken)
            {
                var result = await _productRepository.GetProduct(request.Id);

                return result ?? throw new ProductNotFoundException(request.Id.ToString());
            }
        }
    }
}