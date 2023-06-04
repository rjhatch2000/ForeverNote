using ForeverNote.Core.Infrastructure;
using ForeverNote.Web.Framework.Infrastructure.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ForeverNote.Web.Framework.StartupConfigure
{
    /// <summary>
    /// Represents object for the configuring MVC on application startup
    /// </summary>
    public class ForeverNoteMvcStartup : IForeverNoteStartup
    {
        /// <summary>
        /// Add and configure any of the middleware
        /// </summary>
        /// <param name="services">Collection of service descriptors</param>
        /// <param name="configuration">Configuration root of the application</param>
        public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            //add healthChecks
            services.AddForeverNoteHealthChecks();

            //add miniprofiler
            services.AddForeverNoteMiniProfiler();

            //add WebMarkupMin
            services.AddHtmlMinification();

            //add mediatR
            services.AddMediator();

            //adddetection device
            services.AddDetectionDevice();

            //add and configure MVC feature
            services.AddForeverNoteMvc(configuration);

            //add pwa
            services.AddPWA(configuration);

            //add custom redirect result executor
            services.AddForeverNoteRedirectResultExecutor();

        }

        /// <summary>
        /// Configure the using of added middleware
        /// </summary>
        /// <param name="application">Builder for configuring an application's request pipeline</param>
        public void Configure(IApplicationBuilder application)
        {
            //add MiniProfiler
            application.UseProfiler();

            //MVC endpoint routing
            application.UseForeverNoteEndpoints();

            //save log application started
            application.LogApplicationStarted();
        }

        /// <summary>
        /// Gets order of this startup configuration implementation
        /// </summary>
        public int Order {
            //MVC should be loaded last
            get { return 1000; }
        }
    }
}
