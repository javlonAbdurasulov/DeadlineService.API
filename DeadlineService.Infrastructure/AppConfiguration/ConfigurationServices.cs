using DeadlineService.Application.Interfaces.Base;
using DeadlineService.Application.Interfaces.Repostitories;
using DeadlineService.Application.Interfaces.Services;
using DeadlineService.Application.Services;
using DeadlineService.Infrastructure.Data;
using DeadlineService.Infrastructure.Repositories;
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
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = "localhost:6379";
                options.InstanceName = "SampleInstance"; 
            });
            services.AddScoped<IUserRepository,UserRepository>();
            services.AddScoped<ICommentRepository,CommentRepository>();
            services.AddScoped<IOrderRepository,OrderRepository>();
            services.AddScoped<IPersonalInfoRepository,PersonalInfoRepository>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IRedisCacheService, RedisCacheService>();
            services.AddTransient<IPasswordHasher, PasswordHasherService>();
        }
    }
}
