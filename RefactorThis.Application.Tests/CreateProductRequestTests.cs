using AutoFixture;
using FluentAssertions;
using Moq;
using RefactorThis.Domain.Entities;
using RefactorThis.Domain.Interfaces;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace RefactorThis.Application.Tests
{
    public class CreateProductRequestTests
    {
        private Fixture _fixture;
        private Mock<IProductRepository> _productRepository;

        public CreateProductRequestTests()
        {
            _fixture = new Fixture();
            _productRepository = new Mock<IProductRepository>();
        }

        [Fact]
        public async Task GivenProductExists_WhenRequest_ThenReturnProduct()
        {
            // Arrange
            var product = _fixture.Create<Product>();
            _productRepository.Setup(p => p.CreateProductAsync(It.IsAny<Product>())).ReturnsAsync(product);
            var request = new CreateProductRequest { Product = product };
            var handler = new CreateProductRequest.Handler(_productRepository.Object);

            // Act
            var result = await handler.Handle(request, new CancellationToken());

            // Assert
            result.Should().Be(product);
            _productRepository.Verify(p => p.CreateProductAsync(It.Is<Product>(p => p.ShouldBeEquivalentTrue(product))), Times.Once());
        }
    }
}