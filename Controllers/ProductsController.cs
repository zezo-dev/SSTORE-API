using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Store.Repository.Specification.ProductsBuildSpecs;
using Store.Service.HandleResponse;
using Store.Service.Helper;
using Store.Service.Services.ProductService;
using Store.Service.Services.ProductService.dtos;
using STORE.Api.Helper;

namespace STORE.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
           _productService = productService;
        }

        [HttpGet("brands")]
        [Cache(90)]
        public async Task<ActionResult<IReadOnlyList<BrandTypesDetailsDto>>> GetAllBrands()
            => Ok(await _productService.GetAllBrandssAsync());



        [HttpGet("types")]
        public async Task<ActionResult<IReadOnlyList<BrandTypesDetailsDto>>> GetAllTypes()
            => Ok(await _productService.GetAllTypessAsync());


        [HttpGet]
        public async Task<ActionResult<PaginatedResultDto<ProductDetailsDto>>> GetAllProducts([FromQuery]ProductSpecification input)
           => Ok(await _productService.GetAllProductsAsync(input));



        [HttpGet]  
        public async Task<ActionResult<ProductDetailsDto>> GetProduct(int ? id)
        {
            if (id is null)
                return NotFound(new CustomException(400,"id is null"));
            var product =  await _productService.GetProductDetailsByIdAsync(id);
            if (product is null)
                return NotFound(new CustomException(404));
            return Ok(product);

        }
          


    }
}
