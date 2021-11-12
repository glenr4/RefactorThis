using AutoFixture;
using Newtonsoft.Json;
using RefactorThisAPI;
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
        public async Task WhenGetProduct_EndpointsReturnSuccessAndCorrectContentType(string url)
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync(url);

            // Assert
            response.EnsureSuccessStatusCode(); // Status Code 200-299
            Assert.Equal($"{CONTENT_TYPE}; charset=utf-8",
                response.Content.Headers.ContentType.ToString());
        }

        [Fact]
        public async Task WhenPostProduct_EndpointReturnSuccessAndCorrectContentType()
        {
            // Arrange
            var client = _factory.CreateClient();
            var url = "/products";
            var product = _fixture.Create<TestProduct>();
            var content = new StringContent(JsonConvert.SerializeObject(product), Encoding.UTF8, CONTENT_TYPE);

            // Act
            var response = await client.PostAsync(url, content);

            // Assert
            response.EnsureSuccessStatusCode(); // Status Code 200-299
            Assert.Equal($"{CONTENT_TYPE}; charset=utf-8",
                response.Content.Headers.ContentType.ToString());
        }

        [Fact]
        public async Task WhenPutProduct_EndpointReturnSuccessAndCorrectContentType()
        {
            // Arrange
            var client = _factory.CreateClient();
            var product = Utilities.GetSeedingProducts()[0];
            var url = $"/products/{product.Id}";
            var content = new StringContent(JsonConvert.SerializeObject(product), Encoding.UTF8, CONTENT_TYPE);

            // Act
            var response = await client.PutAsync(url, content);

            // Assert
            response.EnsureSuccessStatusCode(); // Status Code 200-299
            Assert.Equal($"{CONTENT_TYPE}; charset=utf-8",
                response.Content.Headers.ContentType.ToString());
        }

        [Fact]
        public async Task WhenDeleteProduct_EndpointReturnSuccessAndCorrectContentType()
        {
            // Arrange
            var client = _factory.CreateClient();
            var product = Utilities.GetSeedingProducts()[0];
            var url = $"/products/{product.Id}";

            // Act
            var response = await client.DeleteAsync(url);

            // Assert
            response.EnsureSuccessStatusCode(); // Status Code 200-299
        }
    }
}