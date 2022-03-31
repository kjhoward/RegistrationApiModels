using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using DevConsulting.RegistrationLoginApi.Client.Services;
using DevConsulting.RegistrationLoginApi.Client.Extensions;
namespace DevConsulting.RegistrationLoginApi.Client.Authorization
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        public async void OnAuthorization(AuthorizationFilterContext context)
        {
            
            // skip authorization if action is decorated with [AllowAnonymous] attribute
            var allowAnonymous = context.ActionDescriptor.EndpointMetadata.OfType<AllowAnonymousAttribute>().Any();
            if (allowAnonymous)
                return;
            try{
                var user = context.HttpContext.Session.GetUser("user");
                // authorization (for in UserRegistrationAPI)
                if (user != null)
                    return;
            }catch(Exception e){
                //TODO: Some logging here
                context.Result = new JsonResult(new { message = "Unauthorized" }) { StatusCode = StatusCodes.Status401Unauthorized };
                return;
            }
            
            //Fallback authorization (for other APIs that won't have the context set)
            //This sets the context going forward so it should only need to be used once until the context is unset again.
            var authService =
            context.HttpContext.RequestServices.GetService(typeof(IAuthorizationService))
                as IAuthorizationService;

            var utils =
            context.HttpContext.RequestServices.GetService(typeof(IJwtUtils))
                as IJwtUtils; 

            var token = context.HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if(utils == null || authService == null || string.IsNullOrEmpty(token)){
                context.Result = new JsonResult(new { message = "Unauthorized" }) { StatusCode = StatusCodes.Status401Unauthorized };
                return;
            }

            if(utils.ValidateToken(token) == null)
                context.Result = new JsonResult(new { message = "Unauthorized" }) { StatusCode = StatusCodes.Status401Unauthorized };
            
            authService.SetContext();
        }
    }
}