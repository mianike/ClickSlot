using ClickSlotWebAPI.Middlewares;
using Microsoft.Extensions.FileProviders;

namespace ClickSlotWebAPI.Startup
{
    public static class ClickSlotWebAPIPipeline
    {
        public static void InitializePipeline(this WebApplication app, WebApplicationBuilder builder)
        {
            app.UseStaticFiles();

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

            app.UseFileServer(new FileServerOptions
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(builder.Environment.ContentRootPath, "frontend")),
                RequestPath = "",
                EnableDefaultFiles = true
            });
        }
    }
}