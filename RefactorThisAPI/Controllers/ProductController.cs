using MediatR;
using Microsoft.AspNetCore.Authorization;
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
    [Authorize]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProductController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Authorize(Roles = "read_product")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public Task<PagedList<Product>> GetAllProducts([FromQuery] QueryParameters qp)

        {
            return _mediator.Send(new GetAllProductsRequest { Page = qp.Page, PostsPerPage = qp.PostsPerPage, Name = qp.Name });
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "read_product")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public Task<Product> GetProduct(Guid id)
        {
            return _mediator.Send(new GetProductRequest { Id = id });
        }

        [HttpPost]
        [Authorize(Roles = "write_product")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public Task<Product> CreateProduct([FromBody] Product product)
        {
            return _mediator.Send(new CreateProductRequest { Product = product });
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "write_product")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public Task<Product> UpdateProduct(Guid id, [FromBody] Product product)
        {
            if (id != product.Id) throw new ProductIdMismatchException(id.ToString());

            return _mediator.Send(new UpdateProductRequest { Product = product });
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "write_product")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public Task DeleteProduct(Guid id)
        {
            return _mediator.Send(new DeleteProductRequest { Id = id });
        }

        [HttpGet("{id}/options")]
        [Authorize(Roles = "read_product")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public Task<PagedList<ProductOption>> GetAllProductOptions(Guid id, [FromQuery] QueryParameters qp)
        {
            return _mediator.Send(new GetAllProductOptionsRequest { ProductId = id, Page = qp.Page, PostsPerPage = qp.PostsPerPage });
        }

        [HttpGet("{id}/options/{optionId}")]
        [Authorize(Roles = "read_product")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public Task<ProductOption> GetProductOption(Guid id, Guid optionId)
        {
            return _mediator.Send(new GetProductOptionRequest { ProductId = id, ProductOptionId = optionId });
        }

        [HttpPost("{id}/options")]
        [Authorize(Roles = "write_product")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public Task<ProductOption> CreateProductOption(Guid id, [FromBody] ProductOption productOption)
        {
            if (id != productOption.ProductId) throw new ProductIdMismatchException(string.Format(ProductIdMismatchException.MessageTemplate, id.ToString()));

            return _mediator.Send(new CreateProductOptionRequest { ProductOption = productOption });
        }

        [HttpPut("{id}/options/{optionId}")]
        [Authorize(Roles = "write_product")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public Task<ProductOption> UpdateProductOption(Guid id, Guid optionId, [FromBody] ProductOption productOption)
        {
            if (id != productOption.ProductId) throw new ProductIdMismatchException(string.Format(ProductIdMismatchException.MessageTemplate, id.ToString()));
            if (optionId != productOption.Id) throw new ProductOptionIdMismatchException(string.Format(ProductOptionIdMismatchException.MessageTemplate, id.ToString()));

            return _mediator.Send(new UpdateProductOptionRequest { ProductOption = productOption });
        }

        [HttpDelete("{id}/options/{optionId}")]
        [Authorize(Roles = "write_product")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public Task DeleteProductOption(Guid id, Guid optionId)
        {
            return _mediator.Send(new DeleteProductOptionRequest { ProductId = id, ProductOptionId = optionId });
        }
    }
}