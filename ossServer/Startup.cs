﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Serialization;
using ossServer.Hubs;
using ossServer.Models;

namespace ossServer
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ossContext>(options => {
                options.UseSqlServer(Configuration.GetConnectionString("docport.hu"));
            });

            //TODO: lehet finomítani - most mindenhonnan hívható minden action
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                  builder => builder.SetIsOriginAllowed((host) => true) // Core 2.2
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials());
            });

            services.AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                .AddJsonOptions(options => {
                    options.SerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
                    options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Include;

                    options.SerializerSettings.ContractResolver = new DefaultContractResolver { NamingStrategy = new OssNamingStrategy() };
                });

            services.AddSignalR(options =>
            {
                options.EnableDetailedErrors = true;
            });
        }

        public class OssNamingStrategy : NamingStrategy
        {
            protected override string ResolvePropertyName(string name)
            {
                return name;
            }
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseCors("CorsPolicy");

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSignalR(routes =>
            {
                routes.MapHub<OssHub>("/osshub");
            });
            app.UseMvc();
        }
    }
}
