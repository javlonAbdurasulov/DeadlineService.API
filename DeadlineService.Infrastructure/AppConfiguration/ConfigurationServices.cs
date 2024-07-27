using DeadlineService.Application.Interfaces.Repostitories;
using DeadlineService.Application.Interfaces.Services;
using DeadlineService.Application.Services;
using DeadlineService.Infrastructure.Data;
using DeadlineService.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeadlineService.Infrastructure.AppConfiguration
{
    public static class ConfigurationServices
    {
        public static void AddApplicationServices(this IServiceCollection services,IConfiguration configuration)
        {
            services.AddDbContext<DSDbContext>(s => s.UseNpgsql(configuration.GetConnectionString("Default")));
            services.AddScoped<IUserRepository,UserRepository>();

            //Добавление сервиса в котором могут сверятся файл
            services.AddTransient<IPasswordHasher, PasswordHasherService>();
        }
    }
}
