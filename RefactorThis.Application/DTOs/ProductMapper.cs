using RefactorThis.Domain.Entities;

namespace RefactorThis.Application.DTOs
{
    public static class ProductMapper
    {
        public static Product FromDto(ProductDto productDto)
        {
            return new Product(productDto.Id, productDto.Name, productDto.Description, productDto.Price, productDto.DeliveryPrice);
        }
    }
}