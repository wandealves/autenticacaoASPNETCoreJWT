using AuthAPI.Domain.Models;
using System;

namespace AuthAPI.Domain.Contracts.Services
{
    public interface IUserService : IDisposable
    {
        User Authenticate(string login, string password);
        User GetByLogin(string login);
        void Register(User user, string confirmPassword);
        void ChangePassword(string login,string newPassword, string confirmNewPassword);
        string ResetPassword(string login);
        void Delete(string login);
    }
}
