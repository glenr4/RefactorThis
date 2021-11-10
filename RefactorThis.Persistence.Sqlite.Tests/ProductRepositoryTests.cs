using AutoFixture;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using RefactorThis.Domain.Entities;
using RefactorThis.Domain.Exceptions;
using System;
using System.Collections.Generic;
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
            var dbName = _fixture.Create<string>();
            var product = _fixture.Create<Product>();

            using (var context = CreateDbContext(dbName))
            {
                context.Products.Add(product);

                context.SaveChanges();
            }

            // Act
            using (var context = CreateDbContext(dbName))
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
            var dbName = _fixture.Create<string>();

            // Act
            using (var context = CreateDbContext(dbName))
            {
                var productRepository = new ProductRepository(context);

                Func<Task> act = async () => await productRepository.GetProductAsync(Guid.NewGuid());

                // Assert
                await act.Should().ThrowExactlyAsync<ProductNotFoundException>();
            }
        }

        [Fact]
        public async Task GivenProductsExist_WhenGetAllProducts_ThenPagedProductsReturned()
        {
            // Arrange
            var dbName = _fixture.Create<string>();
            var products = new List<Product> {
                _fixture.Create<Product>(),
                _fixture.Create<Product>(),
                _fixture.Create<Product>(),
                _fixture.Create<Product>(),
                _fixture.Create<Product>(),
            };

            using (var context = CreateDbContext(dbName))
            {
                context.Products.AddRange(products);

                context.SaveChanges();
            }

            // Act
            using (var context = CreateDbContext(dbName))
            {
                var productRepository = new ProductRepository(context);

                var result = await productRepository.GetAllProductsAsync(page: 1, pageSize: products.Count);

                // Assert
                result.Items.Should().BeEquivalentTo(products);
                result.Should().BeOfType<PagedList<Product>>();
            }
        }

        [Fact]
        public async Task GivenNoProductsExist_WhenGetAllProducts_ThenEmptyPagedProductsReturned()
        {
            // Arrange
            var dbName = _fixture.Create<string>();

            // Act
            using (var context = CreateDbContext(dbName))
            {
                var productRepository = new ProductRepository(context);

                var result = await productRepository.GetAllProductsAsync();

                // Assert
                result.Items.Should().BeEmpty();
                result.Should().BeOfType<PagedList<Product>>();
            }
        }

        [Fact]
        public async Task GivenProductsExist_WhenGetAllProductsWithName_ThenPagedProductsReturnedWithName()
        {
            // Arrange
            var dbName = _fixture.Create<string>();
            var products = new List<Product> {
                _fixture.Create<Product>(),
                _fixture.Create<Product>(),
                _fixture.Create<Product>(),
                _fixture.Create<Product>(),
                _fixture.Create<Product>(),
            };

            using (var context = CreateDbContext(dbName))
            {
                context.Products.AddRange(products);

                context.SaveChanges();
            }

            // Act
            using (var context = CreateDbContext(dbName))
            {
                var productRepository = new ProductRepository(context);

                var result = await productRepository.GetAllProductsAsync(name: products[0].Name);

                // Assert
                result.Items.Should().BeEquivalentTo(new List<Product> { products[0] });
                result.Should().BeOfType<PagedList<Product>>();
            }
        }

        [Fact]
        public async Task GivenProductsExist_WhenGetAllProductsWithNonExistingName_ThenEmptyPagedProductsReturned()
        {
            // Arrange
            var dbName = _fixture.Create<string>();
            var products = new List<Product> {
                _fixture.Create<Product>(),
                _fixture.Create<Product>(),
                _fixture.Create<Product>(),
                _fixture.Create<Product>(),
                _fixture.Create<Product>(),
            };

            using (var context = CreateDbContext(dbName))
            {
                context.Products.AddRange(products);

                context.SaveChanges();
            }

            // Act
            using (var context = CreateDbContext(dbName))
            {
                var productRepository = new ProductRepository(context);

                var result = await productRepository.GetAllProductsAsync(name: _fixture.Create<string>());

                // Assert
                result.Items.Should().BeEmpty();
                result.Should().BeOfType<PagedList<Product>>();
            }
        }

        [Fact]
        public async Task WhenCreateProduct_ThenProductCreated()
        {
            // Arrange
            var dbName = _fixture.Create<string>();
            var product = _fixture.Create<Product>();

            // Act
            using (var context = CreateDbContext(dbName))
            {
                var productRepository = new ProductRepository(context);

                var result = await productRepository.CreateProductAsync(product);

                // Assert
                result.Should().BeEquivalentTo(product);
            }

            using (var context = CreateDbContext(dbName))
            {
                var actualResult = context.Products.Where(p => p.Id == product.Id).FirstOrDefault();
                actualResult.Should().BeEquivalentTo(product);
            }
        }

        [Fact]
        public async Task GivenProductAlreadyExists_WhenCreateProduct_ThenThrowException()
        {
            // Arrange
            var dbName = _fixture.Create<string>();
            var product = _fixture.Create<Product>();

            using (var context = CreateDbContext(dbName))
            {
                context.Products.Add(product);

                context.SaveChanges();
            }

            // Act
            using (var context = CreateDbContext(dbName))
            {
                var productRepository = new ProductRepository(context);

                Func<Task> act = async () => await productRepository.CreateProductAsync(product);

                // Assert
                await act.Should().ThrowExactlyAsync<DbUpdateException>();
            }
        }

        [Fact]
        public async Task GivenProductExists_WhenUpdateProduct_ThenProductUpdated()
        {
            // Arrange
            var dbName = _fixture.Create<string>();
            var existingProduct = _fixture.Create<Product>();

            using (var context = CreateDbContext(dbName))
            {
                context.Add(existingProduct);

                context.SaveChanges();
            }

            // Act
            var product = new Product(existingProduct.Id, _fixture.Create<string>(), _fixture.Create<string>(), _fixture.Create<decimal>(), _fixture.Create<decimal>());

            using (var context = CreateDbContext(dbName))
            {
                var productRepository = new ProductRepository(context);

                var result = await productRepository.UpdateProductAsync(product);

                // Assert
                result.Should().BeEquivalentTo(product);
            }

            using (var context = CreateDbContext(dbName))
            {
                var updatedProduct = context.Products.Where(p => p.Id == existingProduct.Id).FirstOrDefault();

                updatedProduct.Should().BeEquivalentTo(product);
            }
        }

        [Fact]
        public async Task GivenProductDoesNotExist_WhenUpdateProduct_ThenThrowException()
        {
            // Arrange
            var dbName = _fixture.Create<string>();
            var product = _fixture.Create<Product>();

            // Act
            using (var context = CreateDbContext(dbName))
            {
                var productRepository = new ProductRepository(context);

                Func<Task> act = async () => await productRepository.UpdateProductAsync(product);

                // Assert
                await act.Should().ThrowExactlyAsync<DbUpdateConcurrencyException>();
            }
        }

        [Fact]
        public async Task GivenProductExists_WhenDeleteProduct_ThenProductDeleted()
        {
            // Arrange
            var dbName = _fixture.Create<string>();
            var products = new List<Product>{
                _fixture.Create<Product>(),
                _fixture.Create<Product>(),
                _fixture.Create<Product>(),
            };

            using (var context = CreateDbContext(dbName))
            {
                context.AddRange(products);

                context.SaveChanges();
            }

            // Act
            using (var context = CreateDbContext(dbName))
            {
                var productRepository = new ProductRepository(context);

                var result = await productRepository.DeleteProductAsync(products[0].Id);

                // Assert
                result.Should().Be(products[0].Id);
            }

            using (var context = CreateDbContext(dbName))
            {
                var remainingProducts = context.Products.ToList();

                remainingProducts.Should().HaveCount(2);
                remainingProducts.Should().NotContain(products[0]);
            }
        }

        [Fact]
        public async Task GivenProductDoesNotExist_WhenDeleteProduct_ThenThrowException()
        {
            // Arrange
            var dbName = _fixture.Create<string>();
            var products = new List<Product>{
                _fixture.Create<Product>(),
                _fixture.Create<Product>(),
                _fixture.Create<Product>(),
            };

            using (var context = CreateDbContext(dbName))
            {
                context.AddRange(products);

                context.SaveChanges();
            }

            // Act
            using (var context = CreateDbContext(dbName))
            {
                var productRepository = new ProductRepository(context);

                Func<Task> act = async () => await productRepository.DeleteProductAsync(Guid.NewGuid());

                // Assert
                await act.Should().ThrowExactlyAsync<DbUpdateConcurrencyException>();
            }
        }

        private RefactorThisDbContext CreateDbContext(string dbName)
        {
            return new RefactorThisDbContext(
                        new DbContextOptionsBuilder<RefactorThisDbContext>()
                            .UseInMemoryDatabase(databaseName: dbName)
                            .Options);
        }
    }
}