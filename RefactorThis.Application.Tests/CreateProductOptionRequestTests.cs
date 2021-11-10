using AutoFixture;
using FluentAssertions;
using Moq;
using RefactorThis.Domain.Entities;
using RefactorThis.Persistence;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace RefactorThis.Application.Tests
{
    public class CreateProductOptionRequestTests
    {
        private Fixture _fixture;
        private Mock<IProductOptionRepository> _productOptionRepository;

        public CreateProductOptionRequestTests()
        {
            _fixture = new Fixture();
            _productOptionRepository = new Mock<IProductOptionRepository>();
        }

        [Fact]
        public async Task GivenProductExists_WhenRequest_ThenReturnProduct()
        {
            // Arrange
            var productOption = _fixture.Create<ProductOption>();
            _productOptionRepository.Setup(p => p.CreateProductOptionAsync(It.IsAny<ProductOption>())).ReturnsAsync(productOption);
            var request = new CreateProductOptionRequest { ProductOption = productOption };
            var handler = new CreateProductOptionRequest.Handler(_productOptionRepository.Object);

            // Act
            var result = await handler.Handle(request, new CancellationToken());

            // Assert
            result.Should().Be(productOption);
            _productOptionRepository.Verify(p => p.CreateProductOptionAsync(productOption), Times.Once());
        }
    }
}