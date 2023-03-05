using SignalRChatServer.Abstracts;
using Microsoft.AspNetCore.SignalR;
using SignalRChat.Hubs;
using RabbitMQLibrary;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.DependencyInjection;
using System.Configuration;
using RabbitMQLibrary.RabbitMQ.Abstract;

namespace SignalRChatServer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
             builder.Services.AddSingleton<IRepository, Repository>();
            //���������� SignalR
            builder.Services.AddSignalR();
            //������� ������ ����� ����������� ��� RabbitMQ
            var rabbitOptions = new RabbitMQOptions();
            //������ ����� �� ����� � ������ ������������ � ����������
            builder.Services.Configure<RabbitMQOptions>(builder.Configuration.GetSection(RabbitMQOptions.Options));
            builder.Configuration.GetSection(RabbitMQOptions.Options).Bind(rabbitOptions);
            //������ BackgroundService ��� ������������� �������� ��������� � �������� ��� �����
            builder.Services.AddSingleton<IHostedService>(x =>
            ActivatorUtilities.CreateInstance<RabbitMQConsumer>(x, rabbitOptions)
            );          
           
            builder.Services.AddCors(options =>
            {
                options.AddDefaultPolicy(
                    builder =>
                    {
                        builder.WithOrigins("https://localhost:6001", "http://localhost:5001")
                            .AllowAnyHeader()
                            .WithMethods("GET", "POST")
                            .AllowCredentials();
                    });
            });
           
            var app = builder.Build();
            
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseCors();
            app.MapHub<ChatHub>("/chatHub");
  
            app.MapGet("/", () => "<h3>Server Start!</h3>");

            app.Run();
        }
    }
}