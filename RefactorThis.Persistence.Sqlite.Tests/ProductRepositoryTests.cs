using AutoFixture;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using RefactorThis.Domain.Entities;
using RefactorThis.Domain.Exceptions;
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
                var productRepository = new ProductRepository(context);

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
                var productRepository = new ProductRepository(context);

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
                var productRepository = new ProductRepository(context);

                var result = await productRepository.CreateProductAsync(product);

                // Assert
                result.Should().BeEquivalentTo(product);

                var actualResult = context.Products.Where(p => p.Id == product.Id).FirstOrDefault();
                actualResult.Should().BeEquivalentTo(product);
            }
        }

        [Fact]
        public async Task GivenProductAlreadyExists_WhenCreateProduct_ThenThrowException()
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
                var productRepository = new ProductRepository(context);

                Func<Task> act = async () => await productRepository.CreateProductAsync(product);

                // Assert
                await act.Should().ThrowAsync<DbUpdateException>();
            }
        }

        [Fact]
        public async Task GivenProductExists_WhenUpdateProduct_ThenProductUpdated()
        {
            // Arrange
            var existingProduct = _fixture.Create<Product>();

            using (var context = CreateDbContext())
            {
                context.Add(existingProduct);

                context.SaveChanges();
            }

            // Act
            var product = new Product(existingProduct.Id, _fixture.Create<string>(), _fixture.Create<string>(), _fixture.Create<decimal>(), _fixture.Create<decimal>());

            using (var context = CreateDbContext())
            {
                var productRepository = new ProductRepository(context);

                var result = await productRepository.UpdateProductAsync(product);
            }

            // Assert
            using (var context = CreateDbContext())
            {
                var updatedProduct = context.Products.Where(p => p.Id == existingProduct.Id).FirstOrDefault();

                updatedProduct.Should().BeEquivalentTo(product);
            }
        }

        [Fact]
        public async Task GivenProductDoesNotExist_WhenUpdateProduct_ThenThrowException()
        {
            // Arrange
            var product = _fixture.Create<Product>();

            // Act
            using (var context = CreateDbContext())
            {
                var productRepository = new ProductRepository(context);

                Func<Task> act = async () => await productRepository.UpdateProductAsync(product);

                // Assert
                await act.Should().ThrowAsync<DbUpdateConcurrencyException>();
            }
        }

        private RefactorThisDbContext CreateDbContext()
        {
            return new RefactorThisDbContext(
                        new DbContextOptionsBuilder<RefactorThisDbContext>()
                            .UseInMemoryDatabase(databaseName: "inMemDb")
                            .Options);
        }
    }
}