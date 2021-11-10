using FluentAssertions;
using Moq;
using RefactorThis.Persistence;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace RefactorThis.Application.Tests
{
    public class DeleteProductRequestTests
    {
        private Mock<IProductRepository> _productRepository;

        public DeleteProductRequestTests()
        {
            _productRepository = new Mock<IProductRepository>();
        }

        [Fact]
        public async Task GivenProductExists_WhenRequest_ThenReturnProduct()
        {
            // Arrange
            var productId = Guid.NewGuid();
            _productRepository.Setup(p => p.DeleteProductAsync(It.IsAny<Guid>())).ReturnsAsync(productId);
            var request = new DeleteProductRequest { Id = productId };
            var handler = new DeleteProductRequest.Handler(_productRepository.Object);

            // Act
            var result = await handler.Handle(request, new CancellationToken());

            // Assert
            result.Should().Be(productId);
            _productRepository.Verify(p => p.DeleteProductAsync(It.Is<Guid>(p => p.ShouldBeEquivalentTrue(productId))), Times.Once());
        }
    }
}