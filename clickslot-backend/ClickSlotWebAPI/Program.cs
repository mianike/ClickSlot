using ClickSlotWebAPI.Startup;

namespace ClickSlotWebAPI
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            ClickSlotWebAPIModule.ConfigureServices(builder);

            var app = builder.Build();

            await DbInitializer.InitializeDb(app.Services);

            app.InitializePipeline(builder);

            app.Run();
        }
    }
}