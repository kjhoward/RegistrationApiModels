using System.Text.Json.Serialization;
namespace JustHelpDesk.Models
{
    public class UserResource
    {
        public long Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }

        [JsonIgnore]
        public string PasswordHash { get; set; }
    }
}