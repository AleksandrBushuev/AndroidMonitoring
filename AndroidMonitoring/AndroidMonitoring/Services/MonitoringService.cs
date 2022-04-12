using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidMonitoring.Dto;
using AndroidMonitoring.Entities;
using AndroidMonitoring.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AndroidMonitoring.Services
{
    public class MonitoringService
    {
        private readonly ProductRepository _productRepository;
        private readonly PriceRepository _priceRepository;
        public MonitoringService(ProductRepository productRepository , PriceRepository priceRepository)
        {
            _productRepository = productRepository;
            _priceRepository = priceRepository;
        }

        public ProductDto AddOrUpdateProduct (string name, string url)
        {
            ProductEntity product = new ProductEntity()
            {
                Name = name,
                Url = url
            };

            int id = _productRepository.SaveOrUpdate(product);
            ProductEntity productEntity = _productRepository.GetProductById(id);

            ProductDto productDto = new ProductDto()
            {
                Id = productEntity.Id,
                Name = productEntity.Name,
                Url = productEntity.Url
            };

            return productDto;
        }

        public bool DeleteProduct(int productId)
        {
            bool isDeleted = _productRepository.Delete(productId) > 0;               
            return isDeleted;
        }

        public List<ProductDto> GetProducts()
        {
            var products =_productRepository.GetProducts()
                .Select(productEntity => new ProductDto
                {
                    Id = productEntity.Id,
                    Name = productEntity.Name,
                    Url = productEntity.Url
                }).ToList();
            return products;
        }

        public ProductStatisticsDto GetStatistics(ProductDto product)
        {
            var productEntity = _productRepository.GetProductById(product.Id);

            if (productEntity == null)
                throw new KeyNotFoundException($"Не удалось найти продукт по идентификатору {product.Id}");

            List<PriceEntity> priceEntities = _priceRepository.GetPrices()
                .Where(price => price.ProductId == product.Id)               
                .ToList();

            List<PriceDto> prices = priceEntities
                .Select(priceEntitity => new PriceDto {
                    Id = priceEntitity.Id,
                    Date = priceEntitity.Date,
                    Price = priceEntitity.Price
                })
                .OrderBy(price => price.Date)
                .ToList();

            var statistics = new ProductStatisticsDto()
            {
                ProductId = productEntity.Id,
                ProductName = productEntity.Name,
                Prices = prices
            };
            return statistics;
        }


    }
}