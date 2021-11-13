using MediatR;
using RefactorThis.Domain.Entities;
using RefactorThis.Persistence;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace RefactorThis.Application
{
    public class GetProductOptionRequest : IRequest<ProductOption>
    {
        public Guid ProductId { get; set; }
        public Guid ProductOptionId { get; set; }

        public class Handler : IRequestHandler<GetProductOptionRequest, ProductOption>
        {
            private readonly IProductOptionRepository _productOptionRepository;

            public Handler(IProductOptionRepository productOptionRepository)
            {
                _productOptionRepository = productOptionRepository;
            }

            public Task<ProductOption> Handle(GetProductOptionRequest request, CancellationToken cancellationToken)
            {
                return _productOptionRepository.GetProductOptionAsync(request.ProductId, request.ProductOptionId);
            }
        }
    }
}