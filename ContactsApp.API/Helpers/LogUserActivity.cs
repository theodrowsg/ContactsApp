using System;
using System.Security.Claims;
using System.Threading.Tasks;
using ContactsApp.API.Features.Users;
using MediatR;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace ContactsApp.API.Helpers
{
    public class LogUserActivity : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var resultContext = await next();
            var userId = int.Parse(resultContext.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var mediator = resultContext.HttpContext.RequestServices.GetService<IMediator>();
            var user = await mediator.Send(new UserDetailsQuery { Id = userId});
            user.LastActive = DateTime.Now;
            await mediator.Send(new EditUserCommand { User = user});
            

        }
    }
}