
using Reenbit.Providers;
using Reenbit.Services;

namespace Reenbit
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

            builder.Services.AddSingleton<IStorageProvider, AzureStorageProvider>();
            builder.Services.AddSingleton<IUploader, AzureUploader>();
            builder.Services.AddSingleton<IDateTimeOffsetProvider, SystemDateTimeOffsetProvider>();

            builder.Services.AddCors(policyBuilder =>
                policyBuilder.AddDefaultPolicy(policy =>
                    policy.WithOrigins("https://storagetesttask1.z13.web.core.windows.net").AllowAnyMethod().AllowAnyHeader())
            );

            var app = builder.Build();
            app.UseCors();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}