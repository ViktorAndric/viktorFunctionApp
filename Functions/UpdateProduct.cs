using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using viktorFunctionApp.DbContext;
using viktorFunctionApp.Models;

namespace viktorFunctionApp.Functions
{
    public class UpdateProduct
    {
        private readonly ILogger<UpdateProduct> _logger;
        private readonly AppDbContext _context;
        public UpdateProduct(ILogger<UpdateProduct> logger, AppDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        // Input id for the product you want ot update as a parameter.
        // In the body you can update Name and Price. Input the new values and thats it.
        [Function("UpdateProductById")]
        public async Task<IActionResult> UpdateProductById([HttpTrigger(AuthorizationLevel.Function, "put", Route = "product/{id}")] HttpRequest req, Guid id)
        {
            _logger.LogInformation("Update item by id");

            var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var updatedProductData = JsonConvert.DeserializeObject<Product>(requestBody);

            var product = await _context.Products.FirstOrDefaultAsync(x => x.Id == id);
            if (product == null)
            {
                return new NotFoundResult();
            }

            if (updatedProductData.Name != null)
                product.Name = updatedProductData.Name;
            if (updatedProductData.Price != null)
                product.Price = updatedProductData.Price;

            _context.Products.Update(product);
            await _context.SaveChangesAsync();

            return new OkObjectResult(product);
        }
    }
}
