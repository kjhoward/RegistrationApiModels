using System.ComponentModel.DataAnnotations;

namespace DevConsulting.RegistrationLoginApi.Client.Models
{
    public class AuthenticateRequest
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }
}