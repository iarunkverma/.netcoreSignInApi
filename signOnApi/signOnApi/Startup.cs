using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace signOnApi
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
          
            services.AddAuthentication("OAuth")
                .AddJwtBearer("OAuth", config=> {
                    var stringBytes = Encoding.UTF8.GetBytes(Constants.SecertKey);
                    var key = new SymmetricSecurityKey(stringBytes);
                   
                    //configuration to get the token from header
                    config.Events = new JwtBearerEvents()
                    {
                        OnMessageReceived = context =>
                          {
                              if (context.Request.Query.ContainsKey("access_token"))
                              {
                                  context.Token = context.Request.Query["access_token"];
                              }
                              return Task.CompletedTask;
                          }
                    };

                    //configuration to get the token from header
                    config.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidAudience = Constants.Audience,
                        ValidIssuer = Constants.Issuer,
                        IssuerSigningKey = key
                    };

                });
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
