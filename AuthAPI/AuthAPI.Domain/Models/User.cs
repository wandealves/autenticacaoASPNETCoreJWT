using AuthAPI.Domain.Auth;
using AuthAPI.Resource.Validation;
using Newtonsoft.Json;
using System;

namespace AuthAPI.Domain.Models
{
    public class User
    {
        #region Properties
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public string Login { get; private set; }
        public string Email { get; private set; }
        public string Password { get; private set; }
        #endregion

        #region Ctor
        public User() { }

        public User(string name, string email, string login, string Password)
        {
            //this.Id = Guid.NewGuid();
            this.Name = name;
            this.Email = email;
            this.Login = login;
            this.Password = Password;
        }
        #endregion

        #region Methods
        public void SetPassword(string password, string confirmPassword)
        {
            AssertionConcern.AssertArgumentNotNull(password, "senha nula");
            AssertionConcern.AssertArgumentNotNull(confirmPassword, "confirmar senha nula");
            AssertionConcern.AssertArgumentLength(password, 6, 20, "senha fora do intervalo de 6 até 20");
            AssertionConcern.AssertArgumentEquals(password, confirmPassword, "Senha diferente do confirmar senha");

            this.Password = PasswordAssertionConcern.Encrypt(password);
        }

        public string ResetPassword()
        {
            string password = Guid.NewGuid().ToString().Substring(0, 8);
            this.Password = PasswordAssertionConcern.Encrypt(password);

            return password;
        }

        public void ChangeName(string name)
        {
            this.Name = name;
        }

        public void Validate()
        {
            AssertionConcern.AssertArgumentNotNull(this.Name, "nome nulo");
            AssertionConcern.AssertArgumentNotNull(this.Login, "login nul0");

            AssertionConcern.AssertArgumentLength(this.Name, 3, 250, "Nome fora do intervalo de 3 até 250");
            EmailAssertionConcern.AssertIsValid(this.Email);
            PasswordAssertionConcern.AssertIsValid(this.Password);
        }

        public void UserIsValid(string password)
        {
            PasswordAssertionConcern.AssertIsValid(this.Password, PasswordAssertionConcern.Encrypt(password));
        }

        public string GetToken(JwtProvider tokenProvider)
        {
            string json = JsonConvert.SerializeObject(this);
            return tokenProvider.CreateEncoded(json);
        }
        #endregion
    }
}
