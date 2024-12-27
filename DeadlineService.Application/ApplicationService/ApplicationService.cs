using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using MyApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeadlineService.Application.ApplicationService
{
    public static class ApplicationService
    {
        public static void AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHttpClient("MailGunClient", client =>
            {
                client.BaseAddress = new Uri("https://api.mailgun.net/v3");
                client.DefaultRequestHeaders.Add("Accept", "application/json");
            });

            services.AddSingleton<MailgunService>();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    // указывает, будет ли валидироваться издатель при валидации токена
                    ValidateIssuer = true,
                    
                    // строка, представляющая издателя
                    ValidIssuer = configuration.GetSection("Authorization")["Issuer"],
                    
                    // будет ли валидироваться потребитель токена
                    ValidateAudience = true,
                    
                    // установка потребителя токена
                    ValidAudience = configuration.GetSection("Authorization")["Audience"],
                    
                    // будет ли валидироваться время существования
                    ValidateLifetime = true,
                    
                    // установка ключа безопасности
                    IssuerSigningKey = 
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetSection("Authorization")["SecretKey"]??"Aa12@#aA")),
                    
                    // валидация ключа безопасности
                    ValidateIssuerSigningKey = true,
                };
            });
        }
    }
}
