using MediatR;
using RefactorThis.Domain.Entities;
using RefactorThis.Persistence;
using System.Threading;
using System.Threading.Tasks;

namespace RefactorThis.Application
{
    public class UpdateProductRequest : IRequest<Product>
    {
        public Product Product { get; set; }

        public class Handler : IRequestHandler<UpdateProductRequest, Product>
        {
            private readonly IProductRepository _productRepository;

            public Handler(IProductRepository productRepository)
            {
                _productRepository = productRepository;
            }

            public Task<Product> Handle(UpdateProductRequest request, CancellationToken cancellationToken)
            {
                return _productRepository.UpdateProductAsync(request.Product);
            }
        }
    }
}