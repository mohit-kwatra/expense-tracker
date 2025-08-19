
using ExpenseTracker.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ExpenseTracker.Filters
{
    public class ValidateCategoryIdAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);

            if (!context.ActionArguments.TryGetValue("id", out var idObj) || idObj is not int id || id <= 0)
            {
                context.ModelState.AddModelError("Category", "Invalid or missing data");
                var problemDetails = new ValidationProblemDetails(context.ModelState)
                {
                    Status = StatusCodes.Status400BadRequest
                };
                context.Result = new BadRequestObjectResult(problemDetails);
                return;
            }

            var dbContext = context.HttpContext.RequestServices.GetService<AppDbContext>();
            if (dbContext == null)
            {
                context.Result = new StatusCodeResult(StatusCodes.Status500InternalServerError);
                return;
            }

            bool exists = dbContext.Categories.Any(c => c.CategoryId == id);
            if (!exists)
            {
                context.ModelState.AddModelError("Category", "Category does not exist");
                context.Result = new BadRequestObjectResult(new ValidationProblemDetails(context.ModelState));
                return;
            }
        }
    }
}