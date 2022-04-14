using AndroidMonitoring.Common;
using AndroidMonitoring.Dto;
using AndroidMonitoring.Entities;
using AndroidMonitoring.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

        public ProductStatisticsDto GetStatistics(int productId)
        {
            var productEntity = _productRepository.GetProductById(productId);

            if (productEntity == null)
                throw new KeyNotFoundException($"Не удалось найти продукт по идентификатору {productId}");

            List<PriceEntity> priceEntities = _priceRepository.GetPrices()
                .Where(price => price.ProductId == productId)               
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
        
        public async Task<bool> UpdatePricesAsync(int productId, IProductPriceReader priceReader)
        {
            var productEntity = _productRepository.GetProductById(productId);

            if (productEntity == null)
                return false;

            double price = await priceReader.GetPriceAsync(productEntity.Url);
            if (price < 0)
                return false;

            var currentPriceEntity = new PriceEntity()
            {
                Date = DateTime.Today,
                Price = price,
                ProductId = productEntity.Id
            };

            PriceEntity lastPriceEntity = _priceRepository.GetPrices()
                .Where(price => price.ProductId == productEntity.Id)
                .OrderBy(price => price.Date)
                .FirstOrDefault();

            if(IsCanUpdatePrice(currentPriceEntity, lastPriceEntity))
            {
                lastPriceEntity.Price = price;
                _priceRepository.SaveOrUpdate(lastPriceEntity);
            }
            else
            {
                _priceRepository.SaveOrUpdate(currentPriceEntity);
            }
                      
            return true;

        }

        private bool IsCanUpdatePrice(PriceEntity currentPriceEntity, PriceEntity lastPriceEntity)
        {
            if (lastPriceEntity == null)
                return false;

            if (currentPriceEntity.Date != lastPriceEntity.Date)
                return true;

            if (currentPriceEntity.Price != lastPriceEntity.Price)
                return true;

            return false;
        }
    }
}