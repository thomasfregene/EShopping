using Catalog.Core.Entities;
using Catalog.Core.Repositories;
using Catalog.Core.Specs;
using Catalog.Infrastructure.Data;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository, ITypesRepository, IBrandRepository
    {
        private readonly ICatalogContext _context;

        public ProductRepository(ICatalogContext context)
        {
            _context = context;
        }

        public async Task<Pagination<Product>> GetProducts(CatalogSpecParams specParams)
        {
            var builder = Builders<Product>.Filter;
            var filter = builder.Empty;
            if (!string.IsNullOrEmpty(specParams.Search))
            {
                var searchFilter = builder.Regex(x => x.Name, new BsonRegularExpression(specParams.Search));
                filter &= searchFilter;
            }
            if (!string.IsNullOrEmpty(specParams.BrandId))
            {
                var brandFilter = builder.Eq(x => x.Brands.Id, specParams.BrandId);
                filter &= brandFilter;
            }
            if (!string.IsNullOrEmpty(specParams.TypeId))
            {
                var typeFilter = builder.Eq(x => x.Types.Id, specParams.TypeId);
                filter &= typeFilter;
            }

            if (!string.IsNullOrEmpty(specParams.Sort))
            {
                return new Pagination<Product>()
                {
                    PageSize = specParams.PageSize,
                    PageIndex = specParams.PageIndex,
                    Data = await DataFilter(specParams, filter),
                    Count = await _context.Products.CountDocumentsAsync(p => true) //TODO: Need to check while applying with UI
                };
            }
            return new Pagination<Product>()
            {
                PageSize = specParams.PageSize,
                PageIndex = specParams.PageIndex,
                Data = await _context
                    .Products
                    .Find(filter)
                    .Sort(Builders<Product>.Sort.Ascending("Name"))
                    .Skip(specParams.PageSize * (specParams.PageIndex - 1))
                    .Limit(specParams.PageSize)
                    .ToListAsync(),
                Count = await _context.Products.CountDocumentsAsync(p => true)
            };
        }

        private async Task<IReadOnlyList<Product>> DataFilter(CatalogSpecParams specParams, FilterDefinition<Product> filter)
        {
            switch (specParams.Sort)
            {
                case "priceAsc":
                    return await _context
                    .Products
                    .Find(filter)
                    .Sort(Builders<Product>.Sort.Ascending("Price"))
                    .Skip(specParams.PageSize * (specParams.PageIndex - 1))
                    .Limit(specParams.PageSize)
                    .ToListAsync();
                case "priceDesc":
                    return await _context
                    .Products
                    .Find(filter)
                    .Sort(Builders<Product>.Sort.Descending("Price"))
                    .Skip(specParams.PageSize * (specParams.PageIndex - 1))
                    .Limit(specParams.PageSize)
                    .ToListAsync();
                default:
                    return await _context
                   .Products
                   .Find(filter)
                   .Sort(Builders<Product>.Sort.Ascending("Name"))
                   .Skip(specParams.PageSize * (specParams.PageIndex - 1))
                   .Limit(specParams.PageSize)
                   .ToListAsync();
            }
        }

        public async Task<Product> GetProuct(string id)
        {
            return await _context.Products
                .Find(p => p.Id == id)
                .FirstOrDefaultAsync();
        }
        public async Task<IEnumerable<Product>> GetProductByName(string name)
        {
            FilterDefinition<Product> filter = Builders<Product>.Filter.Eq(p => p.Name, name);
            return await _context.Products
                .Find(filter)
                .ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetProductByBrand(string brand)
        {
            FilterDefinition<Product> filter = Builders<Product>.Filter.Eq(p => p.Brands.Name, brand);
            return await _context.Products
                .Find(filter)
                .ToListAsync();
        }
        public async Task<Product> CreateProduct(Product product)
        {
            await _context.Products.InsertOneAsync(product);
            return product;
        }


        public async Task<bool> UpdateProduct(Product product)
        {
            var updateResult = await _context
                .Products
                .ReplaceOneAsync(p => p.Id == product.Id, product);
            return updateResult.IsAcknowledged && updateResult.ModifiedCount > 0;
        }

        public async Task<bool> DeleteProduct(string id)
        {
            FilterDefinition<Product> filter = Builders<Product>.Filter.Eq(p => p.Id, id);
            DeleteResult deleteResult = await _context.Products
                .DeleteOneAsync(filter);
            return deleteResult.IsAcknowledged && deleteResult.DeletedCount > 0;
        }

        public async Task<IEnumerable<ProductType>> GetAllTypes()
        {
            return await _context.Types
                .Find(t => true)
                .ToListAsync();
        }

        public async Task<IEnumerable<ProductBrand>> GetAllBrands()
        {
            return await _context.Brands
                    .Find(b => true)
                    .ToListAsync();

        }
    }
}
