using MediatR;
using RefactorThis.Domain.Entities;
using RefactorThis.Persistence;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace RefactorThis.Application
{
    public class GetAllProductOptionsRequest : IRequest<PagedList<ProductOption>>
    {
        public Guid ProductId { get; set; }
        public int Page { get; set; }
        public int PostsPerPage { get; set; }

        public class Handler : IRequestHandler<GetAllProductOptionsRequest, PagedList<ProductOption>>
        {
            private readonly IProductOptionRepository _productOptionRepository;

            public Handler(IProductOptionRepository productOptionRepository)
            {
                _productOptionRepository = productOptionRepository;
            }

            public Task<PagedList<ProductOption>> Handle(GetAllProductOptionsRequest request, CancellationToken cancellationToken)
            {
                return _productOptionRepository.GetAllProductOptionsAsync(request.ProductId, request.Page, request.PostsPerPage);
            }
        }
    }
}