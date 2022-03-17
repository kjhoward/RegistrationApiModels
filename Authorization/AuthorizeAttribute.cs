using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using DevConsulting.RegistrationLoginApi.Client.Services;
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

            // authorization (for in UserRegistrationAPI)
            if (CanGetUserFromContext(context))
                return;
            
            //Fallback authorization (for other APIs that won't have the context set)
            //This sets the context going forward so it should only need to be used once until the context is unset again.
            var authService =
            context.HttpContext.RequestServices.GetService(typeof(IAuthorizationService))
                as IAuthorizationService; 
            
            if(authService == null)
                context.Result = new JsonResult(new { message = "Unauthorized" }) { StatusCode = StatusCodes.Status401Unauthorized };
            await authService.Authorize(context.HttpContext);
            if (CanGetUserFromContext(context))
                return;


            context.Result = new JsonResult(new { message = "Unauthorized" }) { StatusCode = StatusCodes.Status401Unauthorized };
        }

        private bool CanGetUserFromContext(AuthorizationFilterContext context){
            var user = (UserResource)context.HttpContext.Items["User"];
            return user != null;
        }
    }
}