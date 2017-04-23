using AuthAPI.Domain.Models;
using System;

namespace AuthAPI.Domain.Contracts.Repositories
{
    public interface IUserRepository : IDisposable
    {
        User Get(string login);
        User Get(Guid id);
        void Create(User user);
        void Update(User user);
        void Delete(User user);
    }
}
