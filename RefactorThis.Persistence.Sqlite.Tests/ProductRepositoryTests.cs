using AutoFixture;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using RefactorThis.Domain.Entities;
using RefactorThis.Domain.Exceptions;
using RefactorThis.Persistence.Sqlite.Exceptions;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace RefactorThis.Persistence.Sqlite.Tests
{
    public class ProductRepositoryTests
    {
        private Fixture _fixture;

        public ProductRepositoryTests()
        {
            _fixture = new Fixture();
        }

        [Fact]
        public async Task GivenProductExists_WhenGetProduct_ThenProductReturned()
        {
            // Arrange
            var product = _fixture.Create<Product>();

            using (var context = CreateDbContext())
            {
                context.Products.Add(product);

                context.SaveChanges();
            }

            // Act
            using (var context = CreateDbContext())
            {
                var productRepository = CreateProductRepository(context);

                var result = await productRepository.GetProductAsync(product.Id);

                // Assert
                result.Should().BeEquivalentTo(product);
            }
        }

        [Fact]
        public async Task GivenProductDoesNotExist_WhenGetProduct_ThenThrowException()
        {
            // Arrange

            // Act
            using (var context = CreateDbContext())
            {
                var productRepository = CreateProductRepository(context);

                Func<Task> act = async () => await productRepository.GetProductAsync(Guid.NewGuid());

                // Assert
                await act.Should().ThrowAsync<ProductNotFoundException>();
            }
        }

        [Fact]
        public async Task WhenCreateProduct_ThenProductCreated()
        {
            // Arrange
            var product = _fixture.Create<Product>();

            // Act
            using (var context = CreateDbContext())
            {
                var productRepository = CreateProductRepository(context);

                var result = await productRepository.CreateProductAsync(product);

                // Assert
                result.Should().BeEquivalentTo(product);

                var actualResult = context.Products.Where(p => p.Id == product.Id).FirstOrDefault();
                actualResult.Should().BeEquivalentTo(product);
            }
        }

        [Fact]
        public async Task GivenProductExists_WhenCreateProductOption_ThenProductOptionAddedToProduct()
        {
            // Arrange
            var product = _fixture.Create<Product>();

            using (var context = CreateDbContext())
            {
                context.Products.Add(product);

                context.SaveChanges();
            }

            // Act
            var productOption = new ProductOption(product.Id, _fixture.Create<string>(), _fixture.Create<string>());
            using (var context = CreateDbContext())
            {
                var productRepository = CreateProductRepository(context);

                var result = await productRepository.CreateProductOptionAsync(product.Id, productOption);

                // Assert
                result.Should().BeEquivalentTo(productOption);

                var actualResult = context.ProductOptions.Where(po => po.Id == productOption.Id).FirstOrDefault();
                actualResult.Should().BeEquivalentTo(productOption);
            }
        }

        [Fact]
        public async Task GivenProductIdDoesNotMatch_WhenCreateProductOption_ThenThrowsException()
        {
            // Arrange
            var product = _fixture.Create<Product>();

            using (var context = CreateDbContext())
            {
                context.Products.Add(product);

                context.SaveChanges();
            }

            // Act
            var productOption = new ProductOption(Guid.NewGuid(), _fixture.Create<string>(), _fixture.Create<string>());
            using (var context = CreateDbContext())
            {
                var productRepository = CreateProductRepository(context);

                Func<Task> act = async () => await productRepository.CreateProductOptionAsync(product.Id, productOption);

                // Assert
                await act.Should().ThrowAsync<ProductIdMismatchException>();
            }
        }

        [Fact]
        public async Task GivenProductDoesNotExist_WhenCreateProductOption_ThenThrowsException()
        {
            // Arrange
            var product = _fixture.Create<Product>();

            // Act
            var productOption = new ProductOption(product.Id, _fixture.Create<string>(), _fixture.Create<string>());
            using (var context = CreateDbContext())
            {
                var productRepository = CreateProductRepository(context);

                Func<Task> act = async () => await productRepository.CreateProductOptionAsync(product.Id, productOption);

                // Assert
                await act.Should().ThrowAsync<DbUpdateException>();
            }
        }

        private RefactorThisDbContext CreateDbContext()
        {
            return new RefactorThisDbContext(
                        new DbContextOptionsBuilder<RefactorThisDbContext>()
                            .UseInMemoryDatabase(databaseName: "inMemDb")
                            .Options);
        }

        private ProductRepository CreateProductRepository(RefactorThisDbContext context)
        {
            return new ProductRepository(context);
        }
    }
}