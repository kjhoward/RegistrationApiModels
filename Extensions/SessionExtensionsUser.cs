using DevConsulting.RegistrationLoginApi.Client;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
namespace DevConsulting.RegistrationLoginApi.Client.Extensions;
public static class SessionExtensionsUser
{
        public static UserResource? GetUser(this ISession session, string key){
                var sVal = session.GetString(key);
                if(sVal == null)
                        return null;
                var usr = JsonConvert.DeserializeObject<UserResource>(sVal);
                return usr;
        }
}