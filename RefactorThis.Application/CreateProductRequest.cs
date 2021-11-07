using AutoMapper;
using MediatR;
using RefactorThis.Application.DTOs;
using RefactorThis.Domain.Entities;
using RefactorThis.Domain.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace RefactorThis.Application
{
    public class CreateProductRequest : IRequest<Product>
    {
        public ProductDto Product { get; set; }

        public class Handler : IRequestHandler<CreateProductRequest, Product>
        {
            private readonly IProductRepository _productRepository;

            public Handler(IProductRepository productRepository)
            {
                _productRepository = productRepository;
            }

            public Task<Product> Handle(CreateProductRequest request, CancellationToken cancellationToken)
            {
                var config = new MapperConfiguration(cfg => cfg.CreateMap<ProductDto, Product>());
                var mapper = new Mapper(config);
                var product = mapper.Map<Product>(request.Product);

                return _productRepository.CreateProductAsync(product);
            }
        }
    }
}