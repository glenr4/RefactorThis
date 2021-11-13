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
        [InlineData("/products", 3)]
        [InlineData("/products?name=Product1", 1)]
        public async Task WhenGetProduct_ThenEndpointsReturnSuccessAndCorrectContentType(string url, int expectedResultCount)
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync(url);

            // Assert
            response.EnsureSuccessStatusCode(); // Status Code 200-299
            var responseContent = await DeserializeResponseContentAsync<TestPagedList<Product>>(response.Content);
            responseContent.Items.Should().HaveCount(expectedResultCount);
            Assert.Equal($"{CONTENT_TYPE}; charset=utf-8", response.Content.Headers.ContentType.ToString());
        }

        [Fact]
        public async Task WhenGetProductById_ThenEndpointsReturnSuccessAndCorrectContentType()
        {
            // Arrange
            var client = _factory.CreateClient();
            var id = TestDbSeeding.GetSeedProducts()[0].Id;
            var url = $"/products/{id}";

            // Act
            var response = await client.GetAsync(url);

            // Assert
            response.EnsureSuccessStatusCode(); // Status Code 200-299
            var responseContent = await DeserializeResponseContentAsync<Product>(response.Content);
            responseContent.Id.Should().Be(id);
            Assert.Equal($"{CONTENT_TYPE}; charset=utf-8", response.Content.Headers.ContentType.ToString());
        }

        [Fact]
        public async Task WhenPostProduct_ThenEndpointReturnSuccessAndCorrectContentType()
        {
            // Arrange
            var client = _factory.CreateClient();
            var product = _fixture.Create<Product>();
            var url = "/products";
            var content = new StringContent(JsonConvert.SerializeObject(product), Encoding.UTF8, CONTENT_TYPE);

            // Act
            var response = await client.PostAsync(url, content);

            // Assert
            response.EnsureSuccessStatusCode(); // Status Code 200-299
            var responseContent = await DeserializeResponseContentAsync<Product>(response.Content);
            responseContent.Should().BeEquivalentTo(product);
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
            var responseContent = await DeserializeResponseContentAsync<Product>(response.Content);
            responseContent.Should().BeEquivalentTo(product);
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

        ////////////////////////////////////////////

        [Fact]
        public async Task WhenGetProductOption_ThenEndpointsReturnSuccessAndCorrectContentType()
        {
            // Arrange
            var client = _factory.CreateClient();
            var productId = TestDbSeeding.GetSeedProductOptions()[0].ProductId;
            var url = $"/products/{productId}/options";
            var expectedResultCount = 1;

            // Act
            var response = await client.GetAsync(url);

            // Assert
            response.EnsureSuccessStatusCode(); // Status Code 200-299
            var responseContent = await DeserializeResponseContentAsync<TestPagedList<ProductOption>>(response.Content);
            responseContent.Items.Should().HaveCount(expectedResultCount);
            responseContent.Items[0].ProductId.Should().Be(productId);
            Assert.Equal($"{CONTENT_TYPE}; charset=utf-8", response.Content.Headers.ContentType.ToString());
        }

        [Fact]
        public async Task WhenGetProductOptionById_ThenEndpointsReturnSuccessAndCorrectContentType()
        {
            // Arrange
            var client = _factory.CreateClient();
            var productId = TestDbSeeding.GetSeedProductOptions()[0].ProductId;
            var productOptionId = TestDbSeeding.GetSeedProductOptions()[0].Id;

            var url = $"/products/{productId}/options/{productOptionId}";

            // Act
            var response = await client.GetAsync(url);

            // Assert
            response.EnsureSuccessStatusCode(); // Status Code 200-299
            var responseContent = await DeserializeResponseContentAsync<ProductOption>(response.Content);
            responseContent.ProductId.Should().Be(productId);
            responseContent.Id.Should().Be(productOptionId);
            Assert.Equal($"{CONTENT_TYPE}; charset=utf-8", response.Content.Headers.ContentType.ToString());
        }

        [Fact]
        public async Task WhenGetProductOptionByIdDoesNotHaveCorrectProductId_ThenReturnStatus404AndCorrectContentType()
        {
            // Arrange
            var client = _factory.CreateClient();
            var productId = Guid.NewGuid();
            var productOptionId = Guid.NewGuid();

            var url = $"/products/{productId}/options/{productOptionId}";

            // Act
            var response = await client.GetAsync(url);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
            Assert.Equal($"{CONTENT_TYPE}; charset=utf-8", response.Content.Headers.ContentType.ToString());
        }

        [Fact]
        public async Task WhenPostProductOption_ThenEndpointReturnSuccessAndCorrectContentType()
        {
            // Arrange
            var client = _factory.CreateClient();
            var productId = TestDbSeeding.GetSeedProducts()[0].Id;
            var productOption = new ProductOption(Guid.NewGuid(), productId, _fixture.Create<string>(), _fixture.Create<string>());
            var url = $"/products/{productId}/options";
            var content = new StringContent(JsonConvert.SerializeObject(productOption), Encoding.UTF8, CONTENT_TYPE);

            // Act
            var response = await client.PostAsync(url, content);

            // Assert
            response.EnsureSuccessStatusCode(); // Status Code 200-299
            var responseContent = await DeserializeResponseContentAsync<ProductOption>(response.Content);
            responseContent.Should().BeEquivalentTo(productOption);
            Assert.Equal($"{CONTENT_TYPE}; charset=utf-8", response.Content.Headers.ContentType.ToString());
        }

        [Fact]
        public async Task WhenPostProductOptionButProductDoesNotExist_ThenReturnStatus400AndCorrectContentType()
        {
            // Arrange
            var client = _factory.CreateClient();
            var productId = TestDbSeeding.GetSeedProducts()[0].Id;
            var productOption = new ProductOption(Guid.NewGuid(), Guid.NewGuid(), _fixture.Create<string>(), _fixture.Create<string>());
            var url = $"/products/{productId}/options";
            var content = new StringContent(JsonConvert.SerializeObject(productOption), Encoding.UTF8, CONTENT_TYPE);

            // Act
            var response = await client.PostAsync(url, content);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            Assert.Equal($"{CONTENT_TYPE}; charset=utf-8", response.Content.Headers.ContentType.ToString());
        }

        [Fact]
        public async Task WhenPutProductOption_ThenEndpointReturnSuccessAndCorrectContentType()
        {
            // Arrange
            var client = _factory.CreateClient();
            var productOption = TestDbSeeding.GetSeedProductOptions()[0];
            var url = $"/products/{productOption.ProductId}/options/{productOption.Id}";
            var content = new StringContent(JsonConvert.SerializeObject(productOption), Encoding.UTF8, CONTENT_TYPE);

            // Act
            var response = await client.PutAsync(url, content);

            // Assert
            response.EnsureSuccessStatusCode(); // Status Code 200-299
            var responseContent = await DeserializeResponseContentAsync<ProductOption>(response.Content);
            responseContent.Should().BeEquivalentTo(productOption);
            Assert.Equal($"{CONTENT_TYPE}; charset=utf-8", response.Content.Headers.ContentType.ToString());
        }

        [Fact]
        public async Task WhenPutProductOptionButProductIdMismatch_ThenReturnStatus400AndCorrectContentType()
        {
            // Arrange
            var client = _factory.CreateClient();
            var productId = Guid.NewGuid();
            var productOption = TestDbSeeding.GetSeedProductOptions()[0];
            var url = $"/products/{productId}/options/{productOption.Id}";
            var content = new StringContent(JsonConvert.SerializeObject(productOption), Encoding.UTF8, CONTENT_TYPE);

            // Act
            var response = await client.PutAsync(url, content);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            Assert.Equal($"{CONTENT_TYPE}; charset=utf-8", response.Content.Headers.ContentType.ToString());
        }

        [Fact]
        public async Task WhenPutProductOptionButProductOptionIdMismatch_ThenReturnStatus400AndCorrectContentType()
        {
            // Arrange
            var client = _factory.CreateClient();
            var productOptionId = Guid.NewGuid();
            var productOption = TestDbSeeding.GetSeedProductOptions()[0];
            var url = $"/products/{productOption.ProductId}/options/{productOptionId}";
            var content = new StringContent(JsonConvert.SerializeObject(productOption), Encoding.UTF8, CONTENT_TYPE);

            // Act
            var response = await client.PutAsync(url, content);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            Assert.Equal($"{CONTENT_TYPE}; charset=utf-8", response.Content.Headers.ContentType.ToString());
        }

        [Fact]
        public async Task WhenDeleteProductOption_ThenEndpointReturnSuccess()
        {
            // Arrange
            var client = _factory.CreateClient();
            var productOptions = TestDbSeeding.GetSeedProductOptions()[1];
            var url = $"/products/{productOptions.ProductId}/options/{productOptions.Id}";

            // Act
            var response = await client.DeleteAsync(url);

            // Assert
            response.EnsureSuccessStatusCode(); // Status Code 200-299
        }

        [Fact]
        public async Task GivenProductOptionDoesNotExist_WhenDeleteProductOption_ThenReturnStatus404AndCorrectContentType()
        {
            // Arrange
            var client = _factory.CreateClient();
            var productOption = _fixture.Create<ProductOption>();
            var url = $"/products/{productOption.ProductId}/options/{productOption.ProductId}";

            // Act
            var response = await client.DeleteAsync(url);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
            Assert.Equal($"{CONTENT_TYPE}; charset=utf-8", response.Content.Headers.ContentType.ToString());
        }

        private async Task<T> DeserializeResponseContentAsync<T>(HttpContent content)
        {
            var stringContext = await content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<T>(stringContext);
        }
    }
}