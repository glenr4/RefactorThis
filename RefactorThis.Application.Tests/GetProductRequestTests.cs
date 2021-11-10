using AutoFixture;
using FluentAssertions;
using Moq;
using RefactorThis.Domain.Entities;
using RefactorThis.Persistence;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace RefactorThis.Application.Tests
{
    public class GetProductRequestTests
    {
        private Fixture _fixture;
        private Mock<IProductRepository> _productRepository;

        public GetProductRequestTests()
        {
            _fixture = new Fixture();
            _productRepository = new Mock<IProductRepository>();
        }

        [Fact]
        public async Task GivenProductExists_WhenRequest_ThenReturnProduct()
        {
            // Arrange
            var id = Guid.NewGuid();
            var product = _fixture.Create<Product>();
            _productRepository.Setup(p => p.GetProductAsync(It.IsAny<Guid>())).ReturnsAsync(product);
            var request = new GetProductRequest { Id = id };
            var handler = new GetProductRequest.Handler(_productRepository.Object);

            // Act
            var result = await handler.Handle(request, new CancellationToken());

            // Assert
            result.Should().Be(product);
            _productRepository.Verify(p => p.GetProductAsync(id), Times.Once());
        }
    }
}