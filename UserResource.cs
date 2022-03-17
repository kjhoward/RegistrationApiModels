using System.Text.Json.Serialization;
namespace DevConsulting.RegistrationLoginApi.Client
{
    public class UserResource
    {
        public long Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }

        [JsonIgnore]
        public string PasswordHash { 
            get { return _passwordHash.Trim(); }
            set { _passwordHash = value;} 
        }

        private string _passwordHash;
    }
}