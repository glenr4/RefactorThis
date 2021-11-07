using MediatR;
using RefactorThis.Domain.Entities;
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

            public Task<Product> Handle(GetProductRequest request, CancellationToken cancellationToken)
            {
                return _productRepository.GetProductAsync(request.Id);
            }
        }
    }
}