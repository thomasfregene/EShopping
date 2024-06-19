using Amazon.Runtime.Internal.Util;
using Catalog.Application.Mappers;
using Catalog.Application.Queries;
using Catalog.Application.Responses;
using Catalog.Core.Repositories;
using Catalog.Core.Specs;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Application.Handlers
{
    public class GetAllProductHandler : IRequestHandler<GetAllProductQuery, Pagination<ProductResponse>>
    {
        private readonly IProductRepository _productRepository;
        private readonly ILogger<GetAllProductHandler> _logger;

        public GetAllProductHandler(IProductRepository productRepository, ILogger<GetAllProductHandler> logger)
        {
            _productRepository = productRepository;
            _logger = logger;
        }
        public async Task<Pagination<ProductResponse>> Handle(GetAllProductQuery request, CancellationToken cancellationToken)
        {
            var productList = await _productRepository.GetProducts(request.CatalogSpecParams);
            var productResponseList = ProductMapper.Mapper.Map<Pagination<ProductResponse>>(productList);
            _logger.LogDebug("Retrieved Product List. Total Count: {productList}", productResponseList.Count);
            return productResponseList;
        }
    }
}
