using System;

namespace AuthAPI.AuthenticationAPI.ViewModels
{
    public class UserVM
    {
        public string Name { get; set; }
        public string Login { get; set; }
        public string Email { get; set; }
        public string Password { get;set; }
        public string ConfirmPassword { get; set; }
    }
}
