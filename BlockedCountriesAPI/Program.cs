    
using BDAssignment.Application.Interfaces;
using BDAssignment.Application.Services;
using BDAssignment.Presentation.BackgroundServices;

namespace BlockedCountriesAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at 
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddSingleton<CountryBlockService>(); // الخدمة الأساسية
            builder.Services.AddHostedService<TemporalBlockCleanupService>(); // الخلفية
            builder.Services.AddHttpClient<IGeoLookupService, GeoLookupService>();
            builder.Services.AddHttpClient<TestIpApiService>();


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
