using AuthAPI.Business.Services;
using AuthAPI.Domain.Auth;
using AuthAPI.Domain.Contracts.Repositories;
using AuthAPI.Domain.Contracts.Services;
using AuthAPI.Infraestructure.Data;
using AuthAPI.Infraestructure.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace AuthAPI.AuthenticationAPI
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddMvc();
            //Configurações
            services.AddSingleton<IConfiguration>(Configuration);
            //Injeção de Dependêcias
            DependencyInjection(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseMvc();

            // Configurando o serviço de documentação do Swagger
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });
        }

        private void DependencyInjection(IServiceCollection services)
        {
            //Injeção de depêndencia do contexto do Entity Framework 
            services.AddDbContext<AppDataContext>(
                options => options.UseSqlServer(this.Configuration.GetConnectionString("AppConnectionString"), b => b.MigrationsAssembly("AuthAPI.AuthenticationAPI"))
                );
            //Usuário repositorio
            services.AddScoped<IUserRepository, UserRepository>();
            //Usuario Service
            services.AddScoped<IUserService, UserService>();

            //Security
            var key = Encoding.UTF8.GetBytes(Configuration["Auth:SecurityKey"]);
            var expiration = Int32.Parse(Configuration["Auth:Expiration"]);
            services.AddSingleton<JwtSettings>(new JwtSettings(new SymmetricSecurityKey(key), expiration));
            services.AddTransient<JwtSecurityTokenHandler>();
            services.AddTransient<JwtProvider>();

            // Configurando o serviço de documentação do Swagger
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1",
                new Info
                {
                    Title = "Autenticação API",
                    Version = "v1",
                    Description = "Gerador de Token",
                    TermsOfService = "None",
                    Contact = new Contact { Name = "javaes", Email = "wanderson.alves.rodrigues@gmail.com", Url = "https://javaes.wordpress.com/" }
                });

            }

           );

        }
    }
}
