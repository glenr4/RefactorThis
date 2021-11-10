using MediatR;
using RefactorThis.Domain.Entities;
using RefactorThis.Domain.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace RefactorThis.Application
{
    public class CreateProductOptionRequest : IRequest<ProductOption>
    {
        public ProductOption ProductOption { get; set; }

        public class Handler : IRequestHandler<CreateProductOptionRequest, ProductOption>
        {
            private readonly IProductOptionRepository _ProductOptionRepository;

            public Handler(IProductOptionRepository ProductOptionRepository)
            {
                _ProductOptionRepository = ProductOptionRepository;
            }

            public Task<ProductOption> Handle(CreateProductOptionRequest request, CancellationToken cancellationToken)
            {
                return _ProductOptionRepository.CreateProductOptionAsync(request.ProductOption);
            }
        }
    }
}