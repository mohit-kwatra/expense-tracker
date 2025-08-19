
using ExpenseTracker.Data;
using ExpenseTracker.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Filters
{
    public class ValidateUniqueCategoryAttribute : Attribute, IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (!context.ActionArguments.TryGetValue("category", out var value) || value is not Category category)
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

            if (dbContext is null)
            {
                context.Result = new StatusCodeResult(StatusCodes.Status500InternalServerError);
                return;
            }

            bool exists = await dbContext.Categories
                .AnyAsync(c => c.Title == category.Title && c.Type == category.Type);

            if (exists)
            {
                context.ModelState.AddModelError("Category", "Category already exists");
                var problemDetails = new ValidationProblemDetails(context.ModelState);
                context.Result = new ConflictObjectResult(problemDetails);
                return;
            }

            await next();
        }
    }
}