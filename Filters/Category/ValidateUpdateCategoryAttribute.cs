
using ExpenseTracker.Data;
using ExpenseTracker.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Filters
{
    public class ValidateUpdateCategoryAttribute : Attribute, IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (!context.ActionArguments.TryGetValue("id", out var idObj) || idObj is not int id ||
                !context.ActionArguments.TryGetValue("category", out var catObj) || catObj is not Category category || id <= 0)
            {
                context.ModelState.AddModelError("Category", "Invalid or missing data");
                context.Result = new BadRequestObjectResult(new ValidationProblemDetails(context.ModelState));
                return;
            }

            if (id != category.CategoryId)
            {
                context.ModelState.AddModelError("CategoryId", "Id in URL and body do not match");
                context.Result = new BadRequestObjectResult(new ValidationProblemDetails(context.ModelState));
                return;
            }

            var dbContext = context.HttpContext.RequestServices.GetService<AppDbContext>();
            if (dbContext is null)
            {
                context.Result = new StatusCodeResult(StatusCodes.Status500InternalServerError);
                return;
            }

            bool exists = await dbContext.Categories.AnyAsync(c => c.CategoryId == id);
            if (!exists)
            {
                context.ModelState.AddModelError("Category", "Category does not exist");
                context.Result = new BadRequestObjectResult(new ValidationProblemDetails(context.ModelState));
                return;
            }

            bool duplicate = await dbContext.Categories
                .AnyAsync(c => c.CategoryId != id &&
                                c.Title.Equals(category.Title) &&
                                c.Type.Equals(category.Type));

            if (duplicate)
            {
                context.ModelState.AddModelError("Category", "Another category with same title and type exists");
                context.Result = new ConflictObjectResult(new ValidationProblemDetails(context.ModelState));
                return;
            }

            await next();
        }
    } 
}