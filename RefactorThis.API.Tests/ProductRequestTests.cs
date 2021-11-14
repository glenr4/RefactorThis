using AutoFixture;
using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using RefactorThis.Domain.Entities;
using RefactorThis.Persistence.Sqlite;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace RefactorThis.API.Tests
{
    public class ProductRequestTests : IDisposable
    {
        private readonly Fixture _fixture;

        private HttpClient _client;
        private const string CONTENT_TYPE = "application/json";

        public ProductRequestTests()
        {
            _fixture = new Fixture();

            _client = CreateClient();
        }

        [Theory]
        [InlineData("/products", 3)]
        [InlineData("/products?name=Product1", 1)]
        public async Task WhenGetProduct_ThenEndpointsReturnSuccessAndCorrectContentType(string url, int expectedResultCount)
        {
            // Arrange

            // Act
            var response = await _client.GetAsync(url);

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
            var id = TestDbSeeding.GetSeedProducts()[0].Id;
            var url = $"/products/{id}";

            // Act
            var response = await _client.GetAsync(url);

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
            var product = _fixture.Create<Product>();
            var url = "/products";
            var content = new StringContent(JsonConvert.SerializeObject(product), Encoding.UTF8, CONTENT_TYPE);

            // Act
            var response = await _client.PostAsync(url, content);

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
            var product = TestDbSeeding.GetSeedProducts()[0];
            var url = $"/products/{product.Id}";
            var content = new StringContent(JsonConvert.SerializeObject(product), Encoding.UTF8, CONTENT_TYPE);

            // Act
            var response = await _client.PutAsync(url, content);

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
            var product = TestDbSeeding.GetSeedProducts()[1];
            var url = $"/products/{product.Id}";

            // Act
            var response = await _client.DeleteAsync(url);

            // Assert
            response.EnsureSuccessStatusCode(); // Status Code 200-299
        }

        [Fact]
        public async Task GivenProductExists_WhenPostProduct_ThenReturnStatus400AndCorrectContentType()

        {
            // Arrange
            var existingProduct = TestDbSeeding.GetSeedProducts()[0];
            var product = new Product(existingProduct.Id, _fixture.Create<string>(), _fixture.Create<string>(), _fixture.Create<int>(), _fixture.Create<int>());
            var url = "/products";
            var content = new StringContent(JsonConvert.SerializeObject(product), Encoding.UTF8, CONTENT_TYPE);

            // Act
            var response = await _client.PostAsync(url, content);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            Assert.Equal($"{CONTENT_TYPE}; charset=utf-8", response.Content.Headers.ContentType.ToString());
        }

        [Fact]
        public async Task GivenProductIdDoesNotMatch_WhenPutProduct_ThenReturnStatus400AndCorrectContentType()
        {
            // Arrange
            var product = _fixture.Create<Product>();
            var url = $"/products/{Guid.NewGuid()}";
            var content = new StringContent(JsonConvert.SerializeObject(product), Encoding.UTF8, CONTENT_TYPE);

            // Act
            var response = await _client.PutAsync(url, content);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            Assert.Equal($"{CONTENT_TYPE}; charset=utf-8", response.Content.Headers.ContentType.ToString());
        }

        [Fact]
        public async Task GivenProductDoesNotExist_WhenPutProduct_ThenReturnStatus404AndCorrectContentType()
        {
            // Arrange
            var product = _fixture.Create<Product>();
            var url = $"/products/{product.Id}";
            var content = new StringContent(JsonConvert.SerializeObject(product), Encoding.UTF8, CONTENT_TYPE);

            // Act
            var response = await _client.PutAsync(url, content);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
            Assert.Equal($"{CONTENT_TYPE}; charset=utf-8", response.Content.Headers.ContentType.ToString());
        }

        [Fact]
        public async Task GivenProductDoesNotExist_WhenDeleteProduct_ThenReturnStatus404AndCorrectContentType()
        {
            // Arrange
            var product = _fixture.Create<Product>();
            var url = $"/products/{product.Id}";

            // Act
            var response = await _client.DeleteAsync(url);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
            Assert.Equal($"{CONTENT_TYPE}; charset=utf-8", response.Content.Headers.ContentType.ToString());
        }

        ////////////////////////////////////////////

        [Fact]
        public async Task WhenGetProductOption_ThenEndpointsReturnSuccessAndCorrectContentType()
        {
            // Arrange
            var productId = TestDbSeeding.GetSeedProductOptions()[0].ProductId;
            var url = $"/products/{productId}/options";
            var expectedResultCount = 1;

            // Act
            var response = await _client.GetAsync(url);

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
            var productId = TestDbSeeding.GetSeedProductOptions()[0].ProductId;
            var productOptionId = TestDbSeeding.GetSeedProductOptions()[0].Id;

            var url = $"/products/{productId}/options/{productOptionId}";

            // Act
            var response = await _client.GetAsync(url);

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
            var productId = Guid.NewGuid();
            var productOptionId = Guid.NewGuid();

            var url = $"/products/{productId}/options/{productOptionId}";

            // Act
            var response = await _client.GetAsync(url);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
            Assert.Equal($"{CONTENT_TYPE}; charset=utf-8", response.Content.Headers.ContentType.ToString());
        }

        [Fact]
        public async Task WhenPostProductOption_ThenEndpointReturnSuccessAndCorrectContentType()
        {
            // Arrange
            var productId = TestDbSeeding.GetSeedProducts()[0].Id;
            var productOption = new ProductOption(Guid.NewGuid(), productId, _fixture.Create<string>(), _fixture.Create<string>());
            var url = $"/products/{productId}/options";
            var content = new StringContent(JsonConvert.SerializeObject(productOption), Encoding.UTF8, CONTENT_TYPE);

            // Act
            var response = await _client.PostAsync(url, content);

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
            var productId = TestDbSeeding.GetSeedProducts()[0].Id;
            var productOption = new ProductOption(Guid.NewGuid(), Guid.NewGuid(), _fixture.Create<string>(), _fixture.Create<string>());
            var url = $"/products/{productId}/options";
            var content = new StringContent(JsonConvert.SerializeObject(productOption), Encoding.UTF8, CONTENT_TYPE);

            // Act
            var response = await _client.PostAsync(url, content);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            Assert.Equal($"{CONTENT_TYPE}; charset=utf-8", response.Content.Headers.ContentType.ToString());
        }

        [Fact]
        public async Task WhenPutProductOption_ThenEndpointReturnSuccessAndCorrectContentType()
        {
            // Arrange
            var productOption = TestDbSeeding.GetSeedProductOptions()[0];
            var url = $"/products/{productOption.ProductId}/options/{productOption.Id}";
            var content = new StringContent(JsonConvert.SerializeObject(productOption), Encoding.UTF8, CONTENT_TYPE);

            // Act
            var response = await _client.PutAsync(url, content);

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
            var productId = Guid.NewGuid();
            var productOption = TestDbSeeding.GetSeedProductOptions()[0];
            var url = $"/products/{productId}/options/{productOption.Id}";
            var content = new StringContent(JsonConvert.SerializeObject(productOption), Encoding.UTF8, CONTENT_TYPE);

            // Act
            var response = await _client.PutAsync(url, content);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            Assert.Equal($"{CONTENT_TYPE}; charset=utf-8", response.Content.Headers.ContentType.ToString());
        }

        [Fact]
        public async Task WhenPutProductOptionButProductOptionIdMismatch_ThenReturnStatus400AndCorrectContentType()
        {
            // Arrange
            var productOptionId = Guid.NewGuid();
            var productOption = TestDbSeeding.GetSeedProductOptions()[0];
            var url = $"/products/{productOption.ProductId}/options/{productOptionId}";
            var content = new StringContent(JsonConvert.SerializeObject(productOption), Encoding.UTF8, CONTENT_TYPE);

            // Act
            var response = await _client.PutAsync(url, content);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            Assert.Equal($"{CONTENT_TYPE}; charset=utf-8", response.Content.Headers.ContentType.ToString());
        }

        [Fact]
        public async Task WhenDeleteProductOption_ThenEndpointReturnSuccess()
        {
            // Arrange
            var productOptions = TestDbSeeding.GetSeedProductOptions()[1];
            var url = $"/products/{productOptions.ProductId}/options/{productOptions.Id}";

            // Act
            var response = await _client.DeleteAsync(url);

            // Assert
            response.EnsureSuccessStatusCode(); // Status Code 200-299
        }

        [Fact]
        public async Task GivenProductOptionDoesNotExist_WhenDeleteProductOption_ThenReturnStatus404AndCorrectContentType()
        {
            // Arrange
            var productOption = _fixture.Create<ProductOption>();
            var url = $"/products/{productOption.ProductId}/options/{productOption.ProductId}";

            // Act
            var response = await _client.DeleteAsync(url);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
            Assert.Equal($"{CONTENT_TYPE}; charset=utf-8", response.Content.Headers.ContentType.ToString());
        }

        private async Task<T> DeserializeResponseContentAsync<T>(HttpContent content)
        {
            var stringContext = await content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<T>(stringContext);
        }

        private HttpClient CreateClient()
        {
            var builder = new WebHostBuilder()
                .UseStartup<TestStartup>()
                .ConfigureServices(services =>
            {
                // Note: TestStartup is executed after this
                services.AddDbContext<RefactorThisDbContext>(options =>
                {
                    options.UseInMemoryDatabase("InMemoryDbForTesting");
                });

                var sp = services.BuildServiceProvider();

                using (var scope = sp.CreateScope())
                {
                    var scopedServices = scope.ServiceProvider;
                    var db = scopedServices.GetRequiredService<RefactorThisDbContext>();

                    db.Database.EnsureCreated();

                    TestDbSeeding.Init(db);
                }
            });

            var testServer = new TestServer(builder);

            return testServer.CreateClient();
        }

        public void Dispose()
        {
            _client.Dispose();
        }
    }
}