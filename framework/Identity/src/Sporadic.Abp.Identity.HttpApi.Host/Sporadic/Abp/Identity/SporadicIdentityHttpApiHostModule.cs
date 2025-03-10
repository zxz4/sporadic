using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System.Collections.Generic;
using Volo.Abp;
using Volo.Abp.AspNetCore.Serilog;
using Volo.Abp.Autofac;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.MySQL;
using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement.EntityFrameworkCore;
using Volo.Abp.Security.Claims;
using Volo.Abp.Swashbuckle;


namespace Sporadic.Abp.Identity
{
    [DependsOn(
        typeof(AbpAutofacModule),
        typeof(AbpEntityFrameworkCoreMySQLModule),
        typeof(AbpPermissionManagementEntityFrameworkCoreModule),
        typeof(AbpAspNetCoreSerilogModule),
        typeof(AbpSwashbuckleModule),
        typeof(SporadicIdentityHttpApiModule),
        typeof(SporadicIdentityApplicationModule),
        typeof(SporadicIdentityEntityFrameworkCoreModule))]

    public class SporadicIdentityHttpApiHostModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var configuration = context.Services.GetConfiguration();
            var hostingEnvironment = context.Services.GetHostingEnvironment();

            Configure<IdentityOptions>(options => 
            {
                options.User.AllowedUserNameCharacters = string.Empty;
            });

            Configure<AbpDbContextOptions>(options =>
            {
                options.UseMySQL();
            });

            ConfigureAuthentication(context, configuration);
            ConfigureSwaggerServices(context, configuration);


#if DEBUG
            context.Services.AddAlwaysAllowAuthorization();
#endif
        }

        private void ConfigureAuthentication(ServiceConfigurationContext context, IConfiguration configuration)
        {
            context.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.Authority = configuration["AuthServer:Authority"];
                    options.RequireHttpsMetadata = configuration.GetValue<bool>("AuthServer:RequireHttpsMetadata");
                    options.Audience = "Identity";
                });

            context.Services.Configure<AbpClaimsPrincipalFactoryOptions>(options =>
            {
                options.IsDynamicClaimsEnabled = false;
            });
        }

        private static void ConfigureSwaggerServices(ServiceConfigurationContext context, IConfiguration configuration)
        {
            context.Services.AddAbpSwaggerGenWithOAuth(
                configuration["AuthServer:Authority"]!,
                new Dictionary<string, string>
                {
                    {"Identity", "Identity API"}
                },
                options =>
                {
                    options.SwaggerDoc("v1", new OpenApiInfo { Title = "Identity API", Version = "v1" });
                    options.DocInclusionPredicate((docName, description) => true);
                    options.CustomSchemaIds(type => type.FullName);
                });
        }


        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            var app = context.GetApplicationBuilder();
            var env = context.GetEnvironment();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseAbpRequestLocalization(options =>
            {
                options.DefaultRequestCulture = new RequestCulture("zh-Hans");
            });
            app.UseCorrelationId();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseCors();
            app.UseAuthentication();

    
            app.UseUnitOfWork();
            app.UseDynamicClaims();
            app.UseAuthorization();

            app.UseSwagger();
            app.UseAbpSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "Identity API");

                var configuration = context.GetConfiguration();
                options.OAuthClientId(configuration["AuthServer:SwaggerClientId"]);
                options.OAuthScopes("Identity");
            });

            app.UseAuditing();
            app.UseAbpSerilogEnrichers();
            app.UseConfiguredEndpoints();
        }
    }
}
