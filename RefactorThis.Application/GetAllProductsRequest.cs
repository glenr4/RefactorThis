using MediatR;
using RefactorThis.Domain.Entities;
using RefactorThis.Persistence;
using System.Threading;
using System.Threading.Tasks;

namespace RefactorThis.Application
{
    public class GetAllProductsRequest : IRequest<PagedList<Product>>
    {
        public int Page { get; set; }
        public int PostsPerPage { get; set; }

        public class Handler : IRequestHandler<GetAllProductsRequest, PagedList<Product>>
        {
            private readonly IProductRepository _productRepository;

            public Handler(IProductRepository productRepository)
            {
                _productRepository = productRepository;
            }

            public Task<PagedList<Product>> Handle(GetAllProductsRequest request, CancellationToken cancellationToken)
            {
                return _productRepository.GetAllProductsAsync(request.Page, request.PostsPerPage);
            }
        }
    }
}