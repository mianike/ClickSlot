using ClickSlotDAL.Contexts;
using Microsoft.EntityFrameworkCore;

namespace ClickSlotWebAPI.Startup
{
    public static class DbInitializer
    {
        public static async Task InitializeDb(IServiceProvider services)
        {
            using var scope = services.CreateScope();

            var dbContext = scope.ServiceProvider.GetRequiredService<ClickSlotDbContext>();

            dbContext.Database.Migrate();
        }
    }
}