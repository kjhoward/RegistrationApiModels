using Newtonsoft.Json;
using DevConsulting.Common.Models.Extensions;
using Microsoft.AspNetCore.Http;

namespace DevConsulting.RegistrationLoginApi.Client.Services{

    public interface IAuthorizationService{
        public Task<UserResource?> Authorize(string token);
        public Task SetContext();
    }
    public class AuthorizationService : IAuthorizationService
    {
        private readonly HttpClient _httpClient;

        private readonly IJwtUtils _jwtUtils;

        private readonly IHttpContextAccessor _httpContextAccessor;


        public AuthorizationService(HttpClient httpClient, IJwtUtils utils, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _jwtUtils = utils;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<UserResource?> Authorize(string token)
        {
            //Authorization (for outside the UserRegistration API)
            if (token == null)
                return null;
            var userId = _jwtUtils.ValidateToken(token);
            if(userId == null)
                return null;
            var result = await _httpClient.AddTokenToHeader(token).GetAsync($"users/{userId.Value}");
            if(!result.IsSuccessStatusCode)
                return null;
            var response = await result.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<UserResource>(response);
        }

        public async Task SetContext(){
            var token = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            if (token == null)
                return;
            var userSession = await Authorize(token);
            if(userSession == null)
                return;
            _httpContextAccessor.HttpContext.Session.SetString("userid", userSession.Id.ToString());
        }
    }
}