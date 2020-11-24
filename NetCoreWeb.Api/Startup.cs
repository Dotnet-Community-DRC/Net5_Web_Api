using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NetCoreWeb.Api.Models;
using NetCoreWeb.Api.Services;

namespace NetCoreWeb.Api
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
            services.AddDbContext<AppDbContext>(opt =>
            {
                opt.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
            });

            //configure Identity

            services.AddIdentity<IdentityUser, IdentityRole>(opt =>
            {
                opt.Password.RequireDigit = true;
                opt.Password.RequireLowercase = true;
                opt.Password.RequiredLength = 6;
            }).AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();

            services.AddAuthentication(auth =>
            {
                auth.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                auth.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(opt =>
            {
                opt.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidAudience = "http://sudidav.io",
                    ValidIssuer = "http://sudidav.io",
                    RequireExpirationTime = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("This is my key for the Encryption")),
                    ValidateIssuerSigningKey = true
                };
            });

            services.AddScoped<IUserService, UserService>();

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "NetCoreWeb.Api", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "NetCoreWeb.Api v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}