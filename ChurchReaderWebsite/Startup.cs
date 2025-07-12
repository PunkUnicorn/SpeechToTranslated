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

            services.Configure<KestrelServerOptions>(options => options.AllowSynchronousIO = true);
            services.Configure<IISServerOptions>(options => options.AllowSynchronousIO = true);

            services.AddSingleton<ITranslationList, TranslationList>();
            services.AddSingleton<IClientList, ClientList>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env/*, IHubContext<ChatHub> hubContext, ITranslationList translationList, IClientList clientList*/)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseFileServer();

            app.UseRouting();

            app.UseEndpoints(async endpoints =>
            {
                //https://learn.microsoft.com/en-us/aspnet/core/tutorials/min-web-api?view=aspnetcore-8.0&tabs=visual-studio
                endpoints.MapPost("/newtranslation/{countryCode}", async delegate(string countryCode, HttpContext context) {
                    Console.WriteLine($"/newtranslation/{countryCode}");

                    var hubContext = (IHubContext<ChatHub>)context.RequestServices.GetService(typeof(IHubContext<ChatHub>));
                    var translationList = (ITranslationList)context.RequestServices.GetService(typeof(ITranslationList));
                    var clientList = (IClientList)context.RequestServices.GetService(typeof(IClientList));

                    using (var stream = new StreamReader(context.Request.Body))
                    {
                        var message =  stream.ReadToEnd();

                        foreach (var client in clientList.Clients)
                            if (client.Value == countryCode)
                                await hubContext.Clients.Client(client.Key).SendAsync("translation2", countryCode, message);

                        if (!translationList.Translations.Contains(countryCode))
                            translationList.Translations.Add(countryCode);
                    }
                });

                //https://learn.microsoft.com/en-us/aspnet/core/tutorials/min-web-api?view=aspnetcore-8.0&tabs=visual-studio
                endpoints.MapPost("/newincremental/{countryCode}", async delegate (string countryCode, HttpContext context) {
                    Console.WriteLine($"/newincremental/{countryCode}");

                    var hubContext = (IHubContext<ChatHub>)context.RequestServices.GetService(typeof(IHubContext<ChatHub>));
                    var translationList = (ITranslationList)context.RequestServices.GetService(typeof(ITranslationList));
                    var clientList = (IClientList)context.RequestServices.GetService(typeof(IClientList));

                    using (var stream = new StreamReader(context.Request.Body))
                    {
                        var message = stream.ReadToEnd();
                        foreach (var client in clientList.Clients)
                            if (client.Value == countryCode)
                                await hubContext.Clients.Client(client.Key).SendAsync("incremental", countryCode, message);
                    }
                });

                //https://learn.microsoft.com/en-us/aspnet/core/tutorials/min-web-api?view=aspnetcore-8.0&tabs=visual-studio
                endpoints.MapPost("/newabsolute/{countryCode}", async delegate (string countryCode, HttpContext context) {
                    Console.WriteLine($"/newabsolute/{countryCode}");

                    var hubContext = (IHubContext<ChatHub>)context.RequestServices.GetService(typeof(IHubContext<ChatHub>));
                    var translationList = (ITranslationList)context.RequestServices.GetService(typeof(ITranslationList));
                    var clientList = (IClientList)context.RequestServices.GetService(typeof(IClientList));

                    using (var stream = new StreamReader(context.Request.Body))
                    {
                        var message = stream.ReadToEnd();
                        foreach (var client in clientList.Clients)
                            if (client.Value == countryCode)
                                await hubContext.Clients.Client(client.Key).SendAsync("absolute", countryCode, message);
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
