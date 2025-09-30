using Microsoft.AspNetCore.Diagnostics;
using ProdutosApi.Models;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text.Json;

namespace ProdutosApi.Extensions
{
    public static class ApiExceptionMiddlewareExtensions
    {
        public static void ConfigureExceptionHandler(this IApplicationBuilder app) //Adiciona este método de extensão à interface IApplicationBuilder 
        {
            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context => //context contém informações como o Request (o que o cliente enviou) e o Response (o que você vai enviar de volta).
                {
                    context.Response.ContentType = "application/json";

                    // Pega a exceção que ocorreu
                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                    
                    if (contextFeature != null)
                    {
                        // Define o status code
                        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                        // Cria o objeto Error com informações da exceção
                        var error = new Error
                        {
                            StatusCode = context.Response.StatusCode,
                            Message = contextFeature.Error.Message,
                            StackTrace = contextFeature.Error.StackTrace
                        };

                        // Envia a resposta
                        await context.Response.WriteAsync(error.ToString());
                    }
                });
            });
        }
    }
}

