using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ClickSlotDAL.Contexts;

namespace ClickSlotTest
{
    public static class DbContextInitializer
    {
        public static ClickSlotDbContext Initialize(string configPath)
        {
            var configurationBuilder = new ConfigurationBuilder();
            configurationBuilder.AddJsonFile(configPath);
            var configuration = configurationBuilder.Build();

            var connectionString = configuration.GetConnectionString("DefaultConnection");

            var dbOptionsBuilder = new DbContextOptionsBuilder<ClickSlotDbContext>().UseNpgsql(connectionString);
            var dbOptions = dbOptionsBuilder.Options;

            var dbContext = new ClickSlotDbContext(dbOptions);

            return dbContext;
        }
    }
}
