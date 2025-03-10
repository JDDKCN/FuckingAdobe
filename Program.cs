using AdobeBlockListConverter.Configs;
using AdobeBlockListConverter.Interfaces;
using AdobeBlockListConverter.Services;
using Microsoft.Extensions.DependencyInjection;
using Application = AdobeBlockListConverter.Applications.Application;

namespace AdobeBlockListConverter
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            try
            {
                var serviceProvider = ConfigureServices();
                var app = serviceProvider.GetRequiredService<IApplication>();
                await app.RunAsync(args);
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"\r\n未处理的异常：{ex.Message}");
                Console.WriteLine(ex.StackTrace);
                Console.ResetColor();
            }

            if (!(args.Length > 2 && args[2] == "-q"))
            {
                Console.WriteLine($"\r\n按任意键退出。");
                Console.ReadKey();
            }
        }

        private static ServiceProvider ConfigureServices()
        {
            var services = new ServiceCollection();

            services.AddSingleton<IAppConfig, AppConfig>();
            services.AddTransient<IFileService, FileService>();
            services.AddTransient<INetworkService, NetworkService>();
            services.AddTransient<IDataProcessor, DataProcessor>();
            services.AddTransient<IUserInterface, ConsoleUserInterface>();
            services.AddTransient<IApplication, Application>();

            return services.BuildServiceProvider();
        }
    }
}