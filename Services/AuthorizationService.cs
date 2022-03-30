using Newtonsoft.Json;
using DevConsulting.Common.Models.Extensions;
using Microsoft.AspNetCore.Http;

namespace DevConsulting.RegistrationLoginApi.Client.Services{

    public interface IAuthorizationService{
        public void SetContext();
    }
    public class AuthorizationService : IAuthorizationService
    {
        private readonly HttpClient _httpClient;

        private readonly IJwtUtils _jwtUtils;

        private readonly IHttpContextAccessor _httpContextAccessor;


        public AuthorizationService(IJwtUtils utils, IHttpContextAccessor httpContextAccessor)
        {
            _jwtUtils = utils;
            _httpContextAccessor = httpContextAccessor;
        }

        public void SetContext(){
            if(_httpContextAccessor == null || _httpContextAccessor.HttpContext == null)
                return;
            var token = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            if (token == null)
                return;
            var userId = _jwtUtils.ValidateToken(token);
            if(userId == null)
                return;
            _httpContextAccessor.HttpContext.Session.SetString("userid", userId.ToString());
        }
    }
}