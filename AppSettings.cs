namespace DevConsulting.RegistrationLoginApi.Client
{
    public class AppSettings
    {
        public string Secret { get; set; }
        public int? InvitationExpiry {get;set;}
        public string ServiceUsername {get;set;}
        public string ServicePassword {get;set;}
    }
}