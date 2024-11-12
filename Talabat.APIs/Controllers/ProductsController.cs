using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.Core.Entities;
using Talabat.Core.Repositories;

namespace Talabat.APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : APIController
    {
        private readonly IGenericRepository<Product> _productRepo;

        public ProductsController(IGenericRepository<Product> ProductRepo)
        {
            _productRepo = ProductRepo;
        }

        // Get All Products
        // BaseUrl/api/ControllerName ->GET
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            var Products = await _productRepo.GetAllAsync();
            //OkObjectResult result = new OkObjectResult(Products);
            //return result;
            return Ok(Products);
        }
        //Get Product By Id
        // BaseUrl/api/ControllerName/{id} ->GET
        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<Product>>> GetProduct(int id)
        {
            var Product = await _productRepo.GetByIdAsync(id);
            return Ok(Product);
        }

    }
}
