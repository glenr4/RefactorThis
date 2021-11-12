using MediatR;
using Microsoft.AspNetCore.Http;
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
        [ProducesResponseType(StatusCodes.Status200OK)]
        public Task<PagedList<Product>> GetAllProducts([FromQuery] QueryParameters qp)
        {
            return _mediator.Send(new GetAllProductsRequest { Page = qp.Page, PostsPerPage = qp.PostsPerPage, Name = qp.Name });
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public Task<Product> GetProduct(Guid id)
        {
            return _mediator.Send(new GetProductRequest { Id = id });
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public Task<Product> CreateProduct([FromBody] Product product)
        {
            return _mediator.Send(new CreateProductRequest { Product = product });
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public Task<Product> UpdateProduct(Guid id, [FromBody] Product product)
        {
            if (id != product.Id) throw new ProductIdMismatchException(id.ToString());

            return _mediator.Send(new UpdateProductRequest { Product = product });
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public Task DeleteProduct(Guid id)
        {
            return _mediator.Send(new DeleteProductRequest { Id = id });
        }

        [HttpGet("{id}/options")]
        public Task<PagedList<ProductOption>> GetAllProductOptions([FromQuery] QueryParameters qp)
        {
            return _mediator.Send(new GetAllProductOptionsRequest { Page = qp.Page, PostsPerPage = qp.PostsPerPage });
        }

        [HttpGet("{id}/options/{optionId}")]
        public Task<ProductOption> GetProductOption(Guid id, Guid optionId)
        {
            return _mediator.Send(new GetProductOptionRequest { ProductId = id, OptionId = optionId });
        }

        [HttpPost("{id}/options")]
        public Task<ProductOption> CreateProductOption(Guid id, [FromBody] ProductOption productOption)
        {
            if (id != productOption.ProductId) throw new ProductIdMismatchException(id.ToString());

            return _mediator.Send(new CreateProductOptionRequest { ProductOption = productOption });
        }

        [HttpPut("{id}/options/{optionId}")]
        public Task<ProductOption> UpdateProductOption(Guid id, Guid optionId, [FromBody] ProductOption productOption)
        {
            if (id != productOption.Id) throw new ProductIdMismatchException(id.ToString());

            return _mediator.Send(new UpdateProductOptionRequest { ProductOption = productOption });
        }

        [HttpDelete("{id}/options/{optionId}")]
        public Task DeleteProductOption(Guid id, Guid optionId)
        {
            return _mediator.Send(new DeleteProductOptionRequest { Id = id });
        }
    }
}