﻿using ForeverNote.Core.Data;
using ForeverNote.Core.Infrastructure;
using ForeverNote.Services.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ForeverNote.Web.Framework.StartupConfigure
{
    /// <summary>
    /// Represents object for the configuring task on application startup
    /// </summary>
    public class TaskHandlerStartup : IForeverNoteStartup
    {
        /// <summary>
        /// Configure the using of added middleware
        /// </summary>
        /// <param name="application">Builder for configuring an application's request pipeline</param>
        public void Configure(IApplicationBuilder application)
        {

        }

        /// <summary>
        /// Add and configure any of the middleware
        /// </summary>
        /// <param name="services">Collection of service descriptors</param>
        /// <param name="configuration">Configuration root of the application</param>
        public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            //database is already installed, so start scheduled tasks
            if (DataSettingsHelper.DatabaseIsInstalled())
            {
                var typeFinder = new WebAppTypeFinder();
                var scheduleTasks = typeFinder.FindClassesOfType<IScheduleTask>();
                foreach (var task in scheduleTasks)
                {
                    var assemblyName = task.Assembly.GetName().Name;
                    services.AddSingleton<IHostedService, BackgroundServiceTask>(sp =>
                    {
                        return new BackgroundServiceTask($"{task.FullName}, {assemblyName}", sp);
                    });
                }
            }
        }

        /// <summary>
        /// Gets order of this startup configuration implementation
        /// </summary>
        public int Order {
            //task handlers should be loaded last
            get { return 1010; }
        }
    }
}
