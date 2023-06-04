using ForeverNote.Api.Constants;
using ForeverNote.Core.Configuration;
using ForeverNote.Core.Infrastructure;
using Microsoft.AspNetCore.OData;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ForeverNote.Api.Infrastructure
{
    public partial class AuthenticationStartup : IForeverNoteStartup
    {
        public void Configure(IApplicationBuilder application)
        {
            var apiConfig = application.ApplicationServices.GetService<ApiConfig>();
            if (apiConfig.Enabled)
            {
                application.UseCors(Configurations.CorsPolicyName);
            }
        }

        public void ConfigureServices(IServiceCollection services,
            IConfiguration configuration)
        {
            var apiConfig = services.BuildServiceProvider().GetService<ApiConfig>();

            if (apiConfig.Enabled)
            {
                //cors
                services.AddCors(options =>
                {
                    options.AddPolicy(Configurations.CorsPolicyName,
                        builder => builder.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader());
                });

                //Add OData
                //TODO: Need to figure this one out
                ////services.AddOData();
                services.AddODataQueryFilter();
            }
        }
        public int Order => 505;


    }
}
