using DeadlineService.Application.Interfaces.Services;
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
            services.AddScoped<IUserService,UserService>();

        }
    }
}
