using MediatR;
using Microsoft.AspNetCore.Mvc;
using RefactorThis.Application;
using RefactorThis.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RefactorThis.API.Controllers
{
    [Route("[controller]")]
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
        public async Task<Product> Get(Guid id)
        {
            var test = await _mediator.Send(new GetProductRequest { Id = id });

            return test;
        }

        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}