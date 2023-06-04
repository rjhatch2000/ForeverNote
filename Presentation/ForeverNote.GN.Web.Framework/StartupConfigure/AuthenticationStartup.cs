using ForeverNote.Core.Configuration;
using ForeverNote.Core.Data;
using ForeverNote.Core.Infrastructure;
using ForeverNote.Web.Framework.Infrastructure.Extensions;
using ForeverNote.Web.Framework.Middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ForeverNote.Web.Framework.StartupConfigure
{
    /// <summary>
    /// Represents object for the configuring authentication middleware on application startup
    /// </summary>
    public class AuthenticationStartup : IForeverNoteStartup
    {
        /// <summary>
        /// Add and configure any of the middleware
        /// </summary>
        /// <param name="services">Collection of service descriptors</param>
        /// <param name="configuration">Configuration root of the application</param>
        public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            var config = new ForeverNoteConfig();
            configuration.GetSection("ForeverNote").Bind(config);

            //add data protection
            services.AddForeverNoteDataProtection(config);
            //add authentication
            services.AddForeverNoteAuthentication(configuration);
        }

        /// <summary>
        /// Configure the using of added middleware
        /// </summary>
        /// <param name="application">Builder for configuring an application's request pipeline</param>
        public void Configure(IApplicationBuilder application)
        {
            //check whether database is installed
            if (!DataSettingsHelper.DatabaseIsInstalled())
                return;

            //configure authentication
            application.UseForeverNoteAuthentication();

            //set storecontext
            application.UseMiddleware<StoreContextMiddleware>();

            //set workcontext
            application.UseMiddleware<WorkContextMiddleware>();

        }

        /// <summary>
        /// Gets order of this startup configuration implementation
        /// </summary>
        public int Order
        {
            //authentication should be loaded before MVC
            get { return 500; }
        }
    }
}
