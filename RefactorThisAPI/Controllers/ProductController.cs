using MediatR;
using Microsoft.AspNetCore.Mvc;
using RefactorThis.Application;
using RefactorThis.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RefactorThis.API.Controllers
{
    // Normally I would use [Route("[controller]")] but the readme asks for the endpoint to be plural not singular
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
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
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
        public void Put(int id, [FromBody] string value)
        {
        }

        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

        [HttpPost("{id}/options")]
        public Task<ProductOption> CreateProductOption(Guid id, [FromBody] ProductOption productOption)
        {
            return _mediator.Send(new CreateProductOptionRequest { ProductId = id, ProductOption = productOption });
        }
    }
}