using MediatR;
using RefactorThis.Domain.Entities;
using RefactorThis.Persistence;
using System.Threading;
using System.Threading.Tasks;

namespace RefactorThis.Application
{
    public class UpdateProductOptionRequest : IRequest<ProductOption>
    {
        public ProductOption ProductOption { get; set; }

        public class Handler : IRequestHandler<UpdateProductOptionRequest, ProductOption>
        {
            private readonly IProductOptionRepository _productOptionRepository;

            public Handler(IProductOptionRepository productOptionRepository)
            {
                _productOptionRepository = productOptionRepository;
            }

            public Task<ProductOption> Handle(UpdateProductOptionRequest request, CancellationToken cancellationToken)
            {
                return _productOptionRepository.UpdateProductOptionAsync(request.ProductOption);
            }
        }
    }
}