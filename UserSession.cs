using DevConsulting.Common.Models;
namespace DevConsulting.RegistrationLoginApi.Client{
    public static class UserSession{
        public static string Token {get;set;}

        public static EventHandler<UserResource> UserInfoChanged;
        private static UserResource _userInfo;

        public static UserResource UserInfo
        {
            get => _userInfo;
            set
            {
                if (_userInfo != value)
                {
                    _userInfo = value;
                    UserInfoChanged?.Invoke(typeof(ToastError), _userInfo);
                }
            }
        }
    }
}