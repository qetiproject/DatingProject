using DatingApp.Api.Data;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using System;
using DatingApp.Api.Models;

namespace DatingApp.Api.Helpers
{
    public class LogUserActivity : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            ActionExecutedContext resultContext = await next();
            var userId = int.Parse(resultContext.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            IBaseRepository repo = resultContext.HttpContext.RequestServices.GetService<IBaseRepository>();
            User user = await repo.GetUser(userId);
            user.LastActive = DateTime.Now;
            await repo.saveAll();
        }
    }
}
