using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using viktorFunctionApp.DbContext;

namespace viktorFunctionApp.Functions
{
    public class DeleteProductById
    {
        private readonly ILogger<DeleteProductById> _logger;
        private readonly AppDbContext _context;
        public DeleteProductById(ILogger<DeleteProductById> logger, AppDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        //Delete by ID
        // Input id for the product you want ot delete as a parameter.
        [Function("DeleteProductById")]
        public async Task<IActionResult> deleteProductById([HttpTrigger(AuthorizationLevel.Function, "delete", Route = "product/{id}")] HttpRequest req, Guid id)
        {
            _logger.LogInformation("Get item by id");
            var product = await _context.Products.FirstOrDefaultAsync(x => x.Id == id);

            if (product == null)
            {
                return new NotFoundResult();
            }
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return new OkObjectResult(product);
        }
    }
}
