using AutoFixture;
using FluentAssertions;
using Moq;
using RefactorThis.Domain.Entities;
using RefactorThis.Domain.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace RefactorThis.Application.Tests
{
    public class CreateProductOptionRequestTests
    {
        private Fixture _fixture;
        private Mock<IProductRepository> _productRepository;

        public CreateProductOptionRequestTests()
        {
            _fixture = new Fixture();
            _productRepository = new Mock<IProductRepository>();
        }

        [Fact]
        public async Task GivenProductExists_WhenRequest_ThenReturnProduct()
        {
            // Arrange
            var id = Guid.NewGuid();
            var productOption = _fixture.Create<ProductOption>();
            _productRepository.Setup(p => p.CreateProductOptionAsync(It.IsAny<Guid>(), It.IsAny<ProductOption>())).ReturnsAsync(productOption);
            var request = new CreateProductOptionRequest { ProductId = id, ProductOption = productOption };
            var handler = new CreateProductOptionRequest.Handler(_productRepository.Object);

            // Act
            var result = await handler.Handle(request, new CancellationToken());

            // Assert
            result.Should().Be(productOption);
            _productRepository.Verify(p => p.CreateProductOptionAsync(id, productOption), Times.Once());
        }
    }
}