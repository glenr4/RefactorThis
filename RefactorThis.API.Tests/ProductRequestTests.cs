using AutoFixture;
using FluentAssertions;
using Newtonsoft.Json;
using RefactorThis.Domain.Entities;
using RefactorThisAPI;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace RefactorThis.API.Tests
{
    public class ProductRequestTests
    : IClassFixture<TestWebApiFactory<Startup>>
    {
        private readonly TestWebApiFactory<Startup> _factory;
        private readonly Fixture _fixture;
        private const string CONTENT_TYPE = "application/json";

        public ProductRequestTests(TestWebApiFactory<Startup> factory)
        {
            _factory = factory;
            _fixture = new Fixture();
        }

        [Theory]
        [InlineData("/products")]
        [InlineData("/products?name=blah")]
        [InlineData("/products/4E2BC5F2-699A-4C42-802E-CE4B4D2AC000")]
        public async Task WhenGetProduct_ThenEndpointsReturnSuccessAndCorrectContentType(string url)
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync(url);

            // Assert
            response.EnsureSuccessStatusCode(); // Status Code 200-299
            Assert.Equal($"{CONTENT_TYPE}; charset=utf-8", response.Content.Headers.ContentType.ToString());
        }

        [Fact]
        public async Task WhenPostProduct_ThenEndpointReturnSuccessAndCorrectContentType()
        {
            // Arrange
            var client = _factory.CreateClient();
            var product = _fixture.Create<TestProduct>();
            var url = "/products";
            var content = new StringContent(JsonConvert.SerializeObject(product), Encoding.UTF8, CONTENT_TYPE);

            // Act
            var response = await client.PostAsync(url, content);

            // Assert
            response.EnsureSuccessStatusCode(); // Status Code 200-299
            Assert.Equal($"{CONTENT_TYPE}; charset=utf-8", response.Content.Headers.ContentType.ToString());
        }

        [Fact]
        public async Task WhenPutProduct_ThenEndpointReturnSuccessAndCorrectContentType()
        {
            // Arrange
            var client = _factory.CreateClient();
            var product = TestDbSeeding.GetSeedProducts()[0];
            var url = $"/products/{product.Id}";
            var content = new StringContent(JsonConvert.SerializeObject(product), Encoding.UTF8, CONTENT_TYPE);

            // Act
            var response = await client.PutAsync(url, content);

            // Assert
            response.EnsureSuccessStatusCode(); // Status Code 200-299
            Assert.Equal($"{CONTENT_TYPE}; charset=utf-8", response.Content.Headers.ContentType.ToString());
        }

        [Fact]
        public async Task WhenDeleteProduct_ThenEndpointReturnSuccess()
        {
            // Arrange
            var client = _factory.CreateClient();
            var product = TestDbSeeding.GetSeedProducts()[1];
            var url = $"/products/{product.Id}";

            // Act
            var response = await client.DeleteAsync(url);

            // Assert
            response.EnsureSuccessStatusCode(); // Status Code 200-299
        }

        [Fact]
        public async Task GivenProductExists_WhenPostProduct_ThenReturnStatus400AndCorrectContentType()

        {
            // Arrange
            var client = _factory.CreateClient();
            var existingProduct = TestDbSeeding.GetSeedProducts()[0];
            var product = new Product(existingProduct.Id, _fixture.Create<string>(), _fixture.Create<string>(), _fixture.Create<int>(), _fixture.Create<int>());
            var url = "/products";
            var content = new StringContent(JsonConvert.SerializeObject(product), Encoding.UTF8, CONTENT_TYPE);

            // Act
            var response = await client.PostAsync(url, content);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            Assert.Equal($"{CONTENT_TYPE}; charset=utf-8", response.Content.Headers.ContentType.ToString());
        }

        [Fact]
        public async Task GivenProductIdDoesNotMatch_WhenPutProduct_ThenReturnStatus400AndCorrectContentType()
        {
            // Arrange
            var client = _factory.CreateClient();
            var product = _fixture.Create<Product>();
            var url = $"/products/{Guid.NewGuid()}";
            var content = new StringContent(JsonConvert.SerializeObject(product), Encoding.UTF8, CONTENT_TYPE);

            // Act
            var response = await client.PutAsync(url, content);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            Assert.Equal($"{CONTENT_TYPE}; charset=utf-8", response.Content.Headers.ContentType.ToString());
        }

        [Fact]
        public async Task GivenProductDoesNotExist_WhenPutProduct_ThenReturnStatus404AndCorrectContentType()
        {
            // Arrange
            var client = _factory.CreateClient();
            var product = _fixture.Create<Product>();
            var url = $"/products/{product.Id}";
            var content = new StringContent(JsonConvert.SerializeObject(product), Encoding.UTF8, CONTENT_TYPE);

            // Act
            var response = await client.PutAsync(url, content);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
            Assert.Equal($"{CONTENT_TYPE}; charset=utf-8", response.Content.Headers.ContentType.ToString());
        }

        [Fact]
        public async Task GivenProductDoesNotExist_WhenDeleteProduct_ThenReturnStatus404AndCorrectContentType()
        {
            // Arrange
            var client = _factory.CreateClient();
            var product = _fixture.Create<Product>();
            var url = $"/products/{product.Id}";

            // Act
            var response = await client.DeleteAsync(url);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
            Assert.Equal($"{CONTENT_TYPE}; charset=utf-8", response.Content.Headers.ContentType.ToString());
        }
    }
}