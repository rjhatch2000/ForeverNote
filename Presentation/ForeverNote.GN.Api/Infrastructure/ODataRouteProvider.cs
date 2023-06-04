using ForeverNote.Api.Constants;
using ForeverNote.Api.Infrastructure.DependencyManagement;
using ForeverNote.Core.Configuration;
using ForeverNote.Core.Infrastructure;
using ForeverNote.Web.Framework.Mvc.Routing;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;
using System;
using System.Linq;

namespace ForeverNote.Api.Infrastructure
{
    public class ODataRouteProvider : IRouteProvider
    {
        public int Priority => 10;

        public void RegisterRoutes(IEndpointRouteBuilder routeBuilder)
        {
            var apiConfig = routeBuilder.ServiceProvider.GetRequiredService<ApiConfig>();
            if (apiConfig.Enabled)
            {
                //OData
                var serviceProvider = routeBuilder.ServiceProvider;
                IEdmModel model = GetEdmModel(serviceProvider, apiConfig);
                //TODO: Need to enable...
                var s = "need to figure this out";
                ////routeBuilder.Count().Filter().OrderBy().MaxTop(Configurations.MaxLimit);
                ////routeBuilder.MapODataRoute(Configurations.ODataRouteName, Configurations.ODataRoutePrefix, model);
                ////routeBuilder.EnableDependencyInjection();
            }
        }

        private IEdmModel GetEdmModel(IServiceProvider serviceProvider, ApiConfig apiConfig)
        {
            ////var builder = new ODataConventionModelBuilder(serviceProvider);
            var builder = new ODataConventionModelBuilder();
            builder.Namespace = Configurations.ODataModelBuilderNamespace;
            RegisterDependencies(builder, apiConfig);
            return builder.GetEdmModel();
        }

        private void RegisterDependencies(ODataConventionModelBuilder builder, ApiConfig apiConfig)
        {
            var typeFinder = new WebAppTypeFinder();

            //find dependency registrars provided by other assemblies
            var dependencyRegistrars = typeFinder.FindClassesOfType<IDependencyEdmModel>();

            //create and sort instances of dependency registrars
            var instances = dependencyRegistrars
                .Select(dependencyRegistrar => (IDependencyEdmModel)Activator.CreateInstance(dependencyRegistrar))
                .OrderBy(dependencyRegistrar => dependencyRegistrar.Order);

            //register all provided dependencies
            foreach (var dependencyRegistrar in instances)
                dependencyRegistrar.Register(builder, apiConfig);
        }
    }
}
