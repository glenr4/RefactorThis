using MediatR;
using Microsoft.AspNetCore.Mvc;
using RefactorThis.API.Exceptions;
using RefactorThis.Application;
using RefactorThis.Domain.Entities;
using RefactorThis.Persistence;
using System;
using System.Threading.Tasks;

namespace RefactorThis.API.Controllers
{
    [Route("products")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProductController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public  Task<PagedList<Product>> GetAll([FromQuery] QueryParameters qp)
        {
            return _mediator.Send(new GetAllProductsRequest { Page = qp.Page, PostsPerPage = qp.PostsPerPage });
        }

        [HttpGet("{id}")]
        public Task<Product> GetProduct(Guid id)
        {
            return _mediator.Send(new GetProductRequest { Id = id });
        }

        [HttpPost]
        public Task<Product> CreateProduct([FromBody] Product product)
        {
            return _mediator.Send(new CreateProductRequest { Product = product });
        }

        [HttpPut("{id}")]
        public Task<Product> Put(Guid id, [FromBody] Product product)
        {
            if (id != product.Id) throw new ProductIdMismatchException(id.ToString());

            return _mediator.Send(new UpdateProductRequest { Product = product });
        }

        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

        [HttpPost("{id}/options")]
        public Task<ProductOption> CreateProductOption(Guid id, [FromBody] ProductOption productOption)
        {
            if (id != productOption.ProductId) throw new ProductIdMismatchException(id.ToString());

            return _mediator.Send(new CreateProductOptionRequest { ProductOption = productOption });
        }
    }
}