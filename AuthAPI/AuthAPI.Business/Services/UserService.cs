using AuthAPI.Domain.Contracts.Repositories;
using AuthAPI.Domain.Contracts.Services;
using AuthAPI.Domain.Models;
using AuthAPI.Resource.Validation;
using System;

namespace AuthAPI.Business.Services
{
    public class UserService : IUserService
    {
        public IUserRepository _repository;

        public UserService(IUserRepository repository)
        {
            this._repository = repository;
        }

        public User GetByLogin(string login)
        {
            User user = null;

            try
            {
                AssertionConcern.AssertArgumentNotNull(login, "login nulo");

                user = this._repository.Get(login);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return user;
        }

        public void Register(User user, string confirmPassword)
        {
            try
            {
                AssertionConcern.AssertArgumentNotNull(user, "usuário não encontrado");

                user.Validate();
                user.SetPassword(user.Password, confirmPassword);

                this._repository.Create(user);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void ChangePassword(string login, string newPassword, string confirmNewPassword)
        {
            try
            {
                AssertionConcern.AssertArgumentNotNull(login, "login nulo");

                User user = this.GetByLogin(login);

                AssertionConcern.AssertArgumentNotNull(user, "usuário não encontrado");

                user.SetPassword(newPassword, confirmNewPassword);
                this._repository.Update(user);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public string ResetPassword(string login)
        {
            string password = "";
            try
            {
                AssertionConcern.AssertArgumentNotNull(login, "login nulo");

                User user = this.GetByLogin(login);

                AssertionConcern.AssertArgumentNotNull(user, "usuário não encontrado");

                password = user.ResetPassword();
                this._repository.Update(user);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return password;
        }

        public User Authenticate(string login, string password)
        {
            User user = null;
            try
            {
                AssertionConcern.AssertArgumentNotNull(login, "login nulo");
                AssertionConcern.AssertArgumentNotNull(password, "senha nula");

                user = this.GetByLogin(login);
                AssertionConcern.AssertArgumentNotNull(user, "usuário não encontrado");
                user.UserIsValid(password);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return user;
        }

        public void Delete(string login)
        {
            try
            {
                AssertionConcern.AssertArgumentNotNull(login, "login nulo");

                User user = this.GetByLogin(login);

                AssertionConcern.AssertArgumentNotNull(user, "usuário não encontrado");

                this._repository.Delete(user);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void Dispose()
        {
            _repository.Dispose();
        }
    }
}
