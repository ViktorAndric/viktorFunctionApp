using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using viktorFunctionApp.DbContext;
using viktorFunctionApp.Models;

namespace viktorFunctionApp.Functions
{
    public class AddProduct
    {
        private readonly ILogger<AddProduct> _logger;
        private readonly AppDbContext _context;
        public AddProduct(ILogger<AddProduct> logger, AppDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        //Create Product
        //You need to input Name and Price into the body when creating a new product
        [Function("AddProduct")]
        public async Task<IActionResult> addProduct([HttpTrigger(AuthorizationLevel.Function, "post", Route = "product")] HttpRequest req)
        {
            _logger.LogInformation("Add product");
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var product = JsonConvert.DeserializeObject<Product>(requestBody);
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return new OkObjectResult(product);
        }

    }

}
