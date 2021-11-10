using MediatR;
using RefactorThis.Persistence;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace RefactorThis.Application
{
    public class DeleteProductRequest : IRequest<Guid>
    {
        public Guid Id { get; set; }

        public class Handler : IRequestHandler<DeleteProductRequest, Guid>
        {
            private readonly IProductRepository _productRepository;

            public Handler(IProductRepository productRepository)
            {
                _productRepository = productRepository;
            }

            public Task<Guid> Handle(DeleteProductRequest request, CancellationToken cancellationToken)
            {
                return _productRepository.DeleteProductAsync(request.Id);
            }
        }
    }
}