using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Talabat.APIs.DTOs;
using Talabat.APIs.Errors;
using Talabat.Core.Entities;
using Talabat.Core.Repositories;
using Talabat.Core.Specifications;

namespace Talabat.APIs.Controllers
{
    
    public class ProductsController : APIBaseController
    {
        private readonly IGenericRepository<Product> _productRepo;
        private readonly IMapper _mapper;

        public ProductsController(IGenericRepository<Product> ProductRepo ,IMapper mapper)
        {
            _productRepo = ProductRepo;
            _mapper = mapper;
        }

        // Get All Products
        // BaseUrl/api/ControllerName ->GET
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            var Spec = new ProductWithBrandAndTypeSpecifications();
            var Products = await _productRepo.GetAllWithSpecAsync(Spec);
            if(Products == null) 
                return NotFound(new ApiResponse(404));
            var MappedProduct = _mapper.Map<IEnumerable<ProductToReturnDto>>(Products);

            //OkObjectResult result = new OkObjectResult(Products);
            //return result;
            return Ok(MappedProduct);
        }
        //Get Product By Id
        // BaseUrl/api/ControllerName/{id} ->GET
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ProductToReturnDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<Product>>> GetProductById(int id)
        {
            var Spec = new ProductWithBrandAndTypeSpecifications(id);
            var Product = await _productRepo.GetByIdWithSpecAsync(Spec);
            if (Product == null)
                return NotFound(new ApiResponse(404));
            var product =Product.Id = 500;
            var MappedProduct = _mapper.Map<ProductToReturnDto>(product);
            return Ok(MappedProduct);
        }

    }
}
