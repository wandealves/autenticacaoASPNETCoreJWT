using AuthAPI.AuthenticationAPI.ViewModels;
using AuthAPI.Domain.Auth;
using AuthAPI.Domain.Contracts.Services;
using AuthAPI.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;

namespace AuthAPI.AuthenticationAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/Login")]
    public class LoginController : Controller
    {
        private IUserService _service;
        private JwtProvider _tokenProvider;
        private IConfiguration _configuracoes;

        public LoginController(IUserService service, JwtProvider tokenProvider, IConfiguration configuracoes)
        {
            this._service = service;
            this._tokenProvider = tokenProvider;
            this._configuracoes = configuracoes;
        }

        [HttpGet("{login}")]
        public IActionResult Get(string login)
        {
            try
            {
                User user = this._service.GetByLogin(login);

                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("register")]
        public IActionResult Register([FromBody] UserVM user)
        {
            try
            {
                this._service.Register(new User(user.Name, user.Email, user.Login, user.Password), user.ConfirmPassword);

                return Ok
                (
                    new
                    {
                        menssage = "sucesso"
                    }
                );
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("changepassword")]
        public IActionResult ChangePassword([FromBody] ChangePasswordVM changePassword)
        {
            try
            {
                this._service.ChangePassword(changePassword.Login, changePassword.NewPassword, changePassword.ConfirmNewPassword);

                return Ok
               (
                   new
                   {
                       menssage = "sucesso"
                   }
               );
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("resetpassword/{login}")]
        public IActionResult ResetPassword(string login)
        {
            string password = "";
            try
            {
                password = this._service.ResetPassword(login);

                return Ok
               (
                   new
                   {
                       password = password
                   }
               );
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody] UserVM user)
        {
            try
            {
                User us = this._service.Authenticate(user.Login, user.Password);

                string token = us.GetToken(this._tokenProvider);

                return Json(new
                {
                    Login = us.Login,
                    Token = token,
                    Expires = DateTime.UtcNow.AddDays(Int32.Parse(this._configuracoes["Auth:Expiration"]))
                }
               );
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{login}")]
        public IActionResult Delete(string login)
        {
            try
            {
                this._service.Delete(login);

                return Ok
              (
                  new
                  {
                      menssage = "sucesso"
                  }
              );
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
