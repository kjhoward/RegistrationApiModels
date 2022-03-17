using Newtonsoft.Json;
using DevConsulting.Common.Models.Extensions;
using Microsoft.AspNetCore.Http;

namespace DevConsulting.RegistrationLoginApi.Client.Services{

    public interface IAuthorizationService{
        public Task Authorize(HttpContext context);
    }
    public class AuthorizationService : IAuthorizationService
    {
        private readonly HttpClient httpClient;

        private readonly IJwtUtils jwtUtils;


        public AuthorizationService(HttpClient httpClient, IJwtUtils utils)
        {
            this.httpClient = httpClient;
            jwtUtils = utils;
        }
        public async Task Authorize(HttpContext context)
        {
            //Authorization (for outside the UserRegistration API)
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            if (token == null)
                return;
            var userId = jwtUtils.ValidateToken(token);
            if(userId == null)
                return;
            var result = await httpClient.AddTokenToHeader(token).GetAsync($"users/{userId.Value}");
            if(!result.IsSuccessStatusCode)
                return;
            var response = await result.Content.ReadAsStringAsync();
            var userResource =  JsonConvert.DeserializeObject<UserResource>(response);

            //If user wasn't set in the context of whatever API was calling it, set it now
            context.Items["User"] = userResource;
        }
    }
}