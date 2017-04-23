namespace AuthAPI.AuthenticationAPI.ViewModels
{
    public class ChangePasswordVM
    {
        public string Login { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmNewPassword { get; set; }
    }
}
