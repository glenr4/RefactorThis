using MediatR;
using RefactorThis.Domain.Entities;
using RefactorThis.Persistence;
using System.Threading;
using System.Threading.Tasks;

namespace RefactorThis.Application
{
    public class CreateProductRequest : IRequest<Product>
    {
        public Product Product { get; set; }

        public class Handler : IRequestHandler<CreateProductRequest, Product>
        {
            private readonly IProductRepository _productRepository;

            public Handler(IProductRepository productRepository)
            {
                _productRepository = productRepository;
            }

            public Task<Product> Handle(CreateProductRequest request, CancellationToken cancellationToken)
            {
                return _productRepository.CreateProductAsync(request.Product);
            }
        }
    }
}