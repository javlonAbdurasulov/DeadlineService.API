using DeadlineService.Application.ApplicationService;
using DeadlineService.Infrastructure.AppConfiguration;
using DeadlineService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
namespace DeadlineService.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Регистрация конфигурации (это уже включено в шаблоне)
            builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            builder.Configuration.AddEnvironmentVariables();

            builder.Services.AddConfigurationServices(builder.Configuration);
            builder.Services.AddApplicationServices(builder.Configuration);

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.MapGet("/confirm", async (string email, string token, DSDbContext dbContext) =>
            {
                // Обработка подтверждения
                var confirmation = await dbContext.EmailConfirmations
                    .FirstOrDefaultAsync(c => c.Email == email && c.Token == token);

                if (confirmation == null || confirmation.Expiration < DateTime.UtcNow)
                    return Results.BadRequest("Invalid or expired confirmation link.");

                var user = await dbContext.Users.Select(x=>x.PersonalInfo).FirstOrDefaultAsync(x=>x.Email==email);
                if (user == null) return Results.BadRequest("User not found.");

                user.isEmailConfirmed = true;
                dbContext.EmailConfirmations.Remove(confirmation);
                await dbContext.SaveChangesAsync();

                return Results.Ok("Email confirmed successfully!");
            });

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
