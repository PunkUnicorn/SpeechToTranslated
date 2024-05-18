using ChatSample.Hubs;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace ChatSample
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSignalR();

            // If using Kestrel:
            services.Configure<KestrelServerOptions>(options =>
            {
                options.AllowSynchronousIO = true;
            });

            // If using IIS:
            services.Configure<IISServerOptions>(options =>
            {
                options.AllowSynchronousIO = true;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseFileServer();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                //https://learn.microsoft.com/en-us/aspnet/core/tutorials/min-web-api?view=aspnetcore-8.0&tabs=visual-studio
                endpoints.MapPost("/newtranslation/{countryCode}", delegate(string countryCode, HttpContext context) {
                    Console.WriteLine($"/newtranslation/{countryCode}");
                   
                    var hubContext = (IHubContext<ChatHub>)context.RequestServices.GetService(typeof(IHubContext<ChatHub>));
                    using (var stream = new StreamReader(context.Request.Body))
                    {
                        var sendResult = hubContext.Clients.All.SendAsync("translation", countryCode, stream.ReadToEnd());
                        Task.WaitAll(new[] {sendResult} );
                    }
                });

                endpoints.MapGet("/wakeup", delegate (HttpContext context) {
                    Console.WriteLine("wakeup");
                });

                endpoints.MapHub<ChatHub>("/chat");
            });
        }
    }
}
