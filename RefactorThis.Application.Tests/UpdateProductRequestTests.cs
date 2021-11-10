using AutoFixture;
using FluentAssertions;
using Moq;
using RefactorThis.Application.DTOs;
using RefactorThis.Domain.Entities;
using RefactorThis.Domain.Interfaces;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace RefactorThis.Application.Tests
{
    public class UpdateProductRequestTests
    {
        private Fixture _fixture;
        private Mock<IProductRepository> _productRepository;

        public UpdateProductRequestTests()
        {
            _fixture = new Fixture();
            _productRepository = new Mock<IProductRepository>();
        }

        [Fact]
        public async Task GivenProductExists_WhenRequest_ThenReturnProduct()
        {
            // Arrange
            var productDto = _fixture.Create<ProductDto>();
            var product = ProductMapper.FromDto(productDto);
            _productRepository.Setup(p => p.UpdateProductAsync(It.IsAny<Product>())).ReturnsAsync(product);
            var request = new UpdateProductRequest { Product = productDto };
            var handler = new UpdateProductRequest.Handler(_productRepository.Object);

            // Act
            var result = await handler.Handle(request, new CancellationToken());

            // Assert
            result.Should().Be(product);
            _productRepository.Verify(p => p.UpdateProductAsync(It.Is<Product>(p => p.ShouldBeEquivalentTrue(product))), Times.Once());
        }
    }
}