using Catalog.Application.Commands;
using Catalog.Application.Queries;
using Catalog.Application.Responses;
using Catalog.Core.Specs;
using Common.Logging.Correlation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Catalog.API.Controllers
{
    public class CatalogController : ApiController
    {
        private readonly IMediator _mediator;
        private readonly ILogger<CatalogController> _logger;
        private readonly ICorrelationIdGenerator _correlationIdGenerator;

        public CatalogController(IMediator mediator, ILogger<CatalogController> logger, ICorrelationIdGenerator correlationIdGenerator)
        {
            _mediator = mediator;
            _logger = logger;
            _correlationIdGenerator = correlationIdGenerator;
            _logger.LogInformation("CorrelationId {correlationId}:", _correlationIdGenerator.Get());
        }

        [HttpGet]
        [Route("[action]/{id}", Name = "GetProductById")]
        [ProducesResponseType(typeof(ProductResponse), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ProductResponse>> GetProductById(string id)
        {
            var query = new GetProductByIdQuery(id);
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet]
        [Route("[action]/{productName}", Name = "GetProductsByName")]
        [ProducesResponseType(typeof(IList<ProductResponse>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IList<ProductResponse>>> GetProductsByName(string productName)
        {
            var query = new GetProductByNameQuery(productName);
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet]   
        [Route("GetAllProducts")]
        [ProducesResponseType(typeof(IList<ProductResponse>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IList<ProductResponse>>> GetAllProducts([FromQuery] CatalogSpecParams catalogSpecParams)
        {
            var query = new GetAllProductQuery(catalogSpecParams);
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet]
        [Route("GetAllBrands")]
        [ProducesResponseType(typeof(IList<BrandResponse>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IList<BrandResponse>>> GetAllBrands()
        {
            var query = new GetAllBrandsQuery();
            var result = await _mediator.Send(query);
            _logger.LogInformation("All product retrieved successfully");
            return Ok(result);
        }

        [HttpGet]
        [Route("GetAllTypes")]
        [ProducesResponseType(typeof(IList<TypesResponse>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IList<TypesResponse>>> GetAllTypes()
        {
            var query = new GetAllTypesQuery();
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet]
        [Route("[action]/{brand}", Name = "GetProductsByBrand")]
        [ProducesResponseType(typeof(IList<ProductResponse>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IList<ProductResponse>>> GetProductsByBrand(string brand)
        {
            var query = new GetProductByBrandQuery(brand);
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpPost]
        [Route("CreateProduct")]
        [ProducesResponseType(typeof(ProductResponse), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ProductResponse>> CreateProduct([FromBody] CreateProductCommand productCommand)
        {
            var result = await _mediator.Send(productCommand);
            return Ok(result);
        }

        [HttpPost]
        [Route("UpdateProduct")]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> UpdateProduct([FromBody] UpdateProductCommand productCommand)
        {
            var result = await _mediator.Send(productCommand);
            return Ok(result);
        }

        [HttpDelete]
        [Route("{id}", Name = "DeleteProduct")]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> DeleteProduct(string id)
        {
            var command = new DeleteProductByIdCommand(id);
            var result = await _mediator.Send(command);
            return Ok(result);
        }
    }
}
