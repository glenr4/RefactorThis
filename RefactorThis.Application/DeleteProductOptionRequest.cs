using MediatR;
using RefactorThis.Persistence;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace RefactorThis.Application
{
    public class DeleteProductOptionRequest : IRequest<Guid>
    {
        public Guid Id { get; set; }

        public class Handler : IRequestHandler<DeleteProductOptionRequest, Guid>
        {
            private readonly IProductOptionRepository _productOptionRepository;

            public Handler(IProductOptionRepository productOptionRepository)
            {
                _productOptionRepository = productOptionRepository;
            }

            public Task<Guid> Handle(DeleteProductOptionRequest request, CancellationToken cancellationToken)
            {
                return _productOptionRepository.DeleteProductOptionAsync(request.Id);
            }
        }
    }
}