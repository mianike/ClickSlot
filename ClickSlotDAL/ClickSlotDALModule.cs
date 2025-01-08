using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ClickSlotDAL.Services;
using ClickSlotDAL.Contracts.Interfaces;
using ClickSlotDAL.Contexts;

namespace ClickSlotDAL
{
    public static class ClickSlotDALModule
    {
        public static void RegisterModule(IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            services.AddDbContext<ClickSlotDbContext>(
                options => options
                    .UseNpgsql(connectionString));

            services.AddScoped<IUnitOfWork, UnitOfWork>();
        }
    }
}
