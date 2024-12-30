using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Talabat.APIs.DTOs;
using Talabat.APIs.Errors;
using Talabat.APIs.Helpers;
using Talabat.Core.Entities;
using Talabat.Core.Repositories;
using Talabat.Core.Specifications;

namespace Talabat.APIs.Controllers
{
    
    public class ProductsController : APIBaseController
    {
        private readonly IGenericRepository<Product> _productRepo;
        private readonly IGenericRepository<ProductType> _typeRepo;
        private readonly IGenericRepository<ProductBrand> _brandRepo;
        private readonly IMapper _mapper;

        public ProductsController(IGenericRepository<Product> ProductRepo, IGenericRepository<ProductType> TypeRepo, IGenericRepository<ProductBrand> BrandRepo, IMapper mapper)
        {
            _productRepo = ProductRepo;
            _typeRepo = TypeRepo;
            _brandRepo = BrandRepo;
            _mapper = mapper;
        }

        // Get All Products
        // BaseUrl/api/ControllerName ->GET
        [HttpGet]
        public async Task<ActionResult<Pagination<ProductToReturnDto>>> GetProducts([FromQuery]ProductSpecParams Params)
        {
            var Spec = new ProductWithBrandAndTypeSpecifications(Params);
            var Products = await _productRepo.GetAllWithSpecAsync(Spec);
            if(Products == null) 
                return NotFound(new ApiResponse(404));
            var MappedProduct = _mapper.Map<IReadOnlyList<ProductToReturnDto>>(Products);
            var CountSpec = new ProductWithFilterationForCountAsync(Params);
            var Count = await _productRepo.GetCountOfSpecAsync(CountSpec);
            return Ok(new Pagination<ProductToReturnDto>(Params.PageSize,Params.PageIndex, Count, MappedProduct));
        }
        //Get Product By Id
        // BaseUrl/api/ControllerName/{id} ->GET
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ProductToReturnDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IReadOnlyList<ProductToReturnDto>>> GetProductById(int id)
        {
            var Spec = new ProductWithBrandAndTypeSpecifications(id);
            var Product = await _productRepo.GetByIdWithSpecAsync(Spec);
            if (Product == null)
                return NotFound(new ApiResponse(404));
            //var product =Product.Id = 500; buggy for server error
            var MappedProduct = _mapper.Map<ProductToReturnDto>(Product);
            return Ok(MappedProduct);
        }
        [HttpGet("Types")]
        public async Task<ActionResult<IReadOnlyList<ProductType>>> GetTypes()
        {
            var Types = await _typeRepo.GetAllAsync();
            return Ok(Types);
        }
        [HttpGet("Brands")]
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetBrands()
        {
            var Brands = await _brandRepo.GetAllAsync();
            return Ok(Brands);
        }

    }
}
