using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Guide.Application.ApplicationServices;
using Guide.Application.CQRS.Command;
using Guide.Application.CQRS.Query;
using Guide.Application.ViewModel.CommandResponses.Product;
using Guide.API.Filter;
using Guide.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Guide.API.Controllers
{
    [Route("api/products")]
    [ValidateModel]
    public class ProductController : BaseController
    {
        private readonly ProductManager _productManager;

        public ProductController(ProductManager productManager)
        {
            _productManager = productManager;
        }

        [HttpGet("", Name = "GetProductsByStockAmount")]
        public IActionResult Get([FromBody] ProductListQuery model)
        {
            var products = _productManager.GetProductsByStockAmount(model);

            return Ok(products);
        }

        [HttpGet("{id}", Name = "GetProduct")]
        public IActionResult Get(int id)
        {
            var result = _productManager.GetProductById(id);
            if (result == null)
                return NotFound();

            return Ok(result);
        }

        [HttpPost("")]
        public IActionResult Post([FromBody] ProductCreateCommand model)
        {
            var result = _productManager.CreateProduct(model);

            return CreatedAtRoute("GetProduct", new { id = result.Id }, result);
        }

        [HttpPut("")]
        public IActionResult Put([FromBody] ProductChangePriceCommand model)
        {
            var result = _productManager.ChangeProductPrice(model);
            return CreatedAtRoute("GetProduct", new { id = result.Id }, result);
        }
    }
}
