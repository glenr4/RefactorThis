using AutoFixture;
using FluentAssertions;
using MockQueryable.Moq;
using Moq;
using RefactorThis.Domain.Entities;
using RefactorThis.Persistence;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace RefactorThis.Application.Tests
{
    public class GetAllProductsRequestTests
    {
        private Fixture _fixture;
        private Mock<IProductRepository> _productRepository;

        public GetAllProductsRequestTests()
        {
            _fixture = new Fixture();
            _productRepository = new Mock<IProductRepository>();
        }

        [Fact]
        public async Task GivenProductExists_WhenRequest_ThenReturnProduct()
        {
            // Arrange
            var products = _fixture.CreateMany<Product>().ToList();
            var mockQueryable = products.AsQueryable().BuildMock();
            var pagedList = PagedList<Product>.ToPagedListAsync(mockQueryable.Object, 1, 1);
            var page = _fixture.Create<int>();
            var postsPerPage = _fixture.Create<int>();
            var name = _fixture.Create<string>();
            _productRepository.Setup(p => p.GetAllProductsAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>())).Returns(pagedList);

            var request = new GetAllProductsRequest { Page = page, PostsPerPage = postsPerPage, Name = name };
            var handler = new GetAllProductsRequest.Handler(_productRepository.Object);

            // Act
            var result = await handler.Handle(request, new CancellationToken());

            // Assert
            result.Should().BeOfType<PagedList<Product>>();
            _productRepository.Verify(p => p.GetAllProductsAsync(page, postsPerPage, name), Times.Once());
        }
    }
}