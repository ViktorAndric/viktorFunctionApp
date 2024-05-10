using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using viktorFunctionApp.DbContext;

namespace viktorFunctionApp.Functions
{
    public class GetProductById
    {
        private readonly ILogger<GetProductById> _logger;
        private readonly AppDbContext _context;
        public GetProductById(ILogger<GetProductById> logger, AppDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        //Get by ID
        // Input id for the product you want ot get as a parameter.
        [Function("GetProductById")]
        public async Task<IActionResult> getProductById([HttpTrigger(AuthorizationLevel.Function, "get", Route = "product/{id}")] HttpRequest req, Guid id)
        {
            _logger.LogInformation("Get item by id");
            var product = await _context.Products.FirstOrDefaultAsync(x => x.Id == id);
            if (product == null)
            {
                return new NotFoundResult();
            }
            return new OkObjectResult(product);
        }
    }
}
