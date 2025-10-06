
using Microsoft.EntityFrameworkCore;
using ProdutosApi.Context;
using ProdutosApi.DTOs.Mappings;
using ProdutosApi.Extensions;
using ProdutosApi.Repositories;

namespace ProdutosApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddAutoMapper(typeof(DTOMappingProfile));
            builder.Services.AddControllers().AddJsonOptions(options=>
                options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles); //Ignora referência cíclica. Olhar vídeo "Ajustes e Otimizações - Serialização JSON" do curso Web Api Essencial para saber mais
            builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
            builder.Services.AddScoped<IProductRepository, ProductRepository>();
            builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            string mySqlConnection = builder.Configuration.GetConnectionString("DefaultConnection");
            
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseMySql(mySqlConnection,
                ServerVersion.AutoDetect(mySqlConnection)));

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
                app.ConfigureExceptionHandler(); //Tratamento de erros inesperados

            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
