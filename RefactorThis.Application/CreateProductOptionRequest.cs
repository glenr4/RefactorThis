using MediatR;
using RefactorThis.Domain.Entities;
using RefactorThis.Domain.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace RefactorThis.Application
{
    public class CreateProductOptionRequest : IRequest<ProductOption>
    {
        public Guid ProductId { get; set; }
        public ProductOption ProductOption { get; set; }

        public class Handler : IRequestHandler<CreateProductOptionRequest, ProductOption>
        {
            private readonly IProductRepository _ProductOptionRepository;

            public Handler(IProductRepository ProductOptionRepository)
            {
                _ProductOptionRepository = ProductOptionRepository;
            }

            public Task<ProductOption> Handle(CreateProductOptionRequest request, CancellationToken cancellationToken)
            {
                return _ProductOptionRepository.CreateProductOption(request.ProductId, request.ProductOption);
            }
        }
    }
}