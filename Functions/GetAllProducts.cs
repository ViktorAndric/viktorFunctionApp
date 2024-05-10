using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using viktorFunctionApp.DbContext;

namespace viktorFunctionApp.Functions
{
    public class GetAllProducts
    {
        private readonly ILogger<GetAllProducts> _logger;
        private readonly AppDbContext _context;
        public GetAllProducts(ILogger<GetAllProducts> logger, AppDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        //Get all
        [Function("GetAllProducts")]
        public async Task<IActionResult> getAllProducts([HttpTrigger(AuthorizationLevel.Function, "get", Route = "product")] HttpRequest req)
        {
            _logger.LogInformation("Get all items");
            var products = await _context.Products.ToListAsync();

            return new OkObjectResult(products);
        }

    }
}
