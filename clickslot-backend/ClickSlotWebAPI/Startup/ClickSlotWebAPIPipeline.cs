using ClickSlotWebAPI.Middlewares;
using Microsoft.Extensions.FileProviders;

namespace ClickSlotWebAPI.Startup
{
    public static class ClickSlotWebAPIPipeline
    {
        public static void InitializePipeline(this WebApplication app, WebApplicationBuilder builder)
        {
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseMiddleware<CurrentAppUserMiddleware>();

            app.MapControllers();

            app.UseCors();
        }
    }
}