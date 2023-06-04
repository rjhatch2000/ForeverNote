using ForeverNote.Core.Configuration;
using Microsoft.OData.ModelBuilder;

namespace ForeverNote.Api.Infrastructure.DependencyManagement
{
    public interface IDependencyEdmModel
    {
        /// <summary>
        /// Register edmmodel
        /// </summary>
        /// <param name="builder">OData Convention Model Builder</param>
        void Register(ODataConventionModelBuilder builder, ApiConfig apiConfig);

        /// <summary>
        /// Order of this dependency registrar implementation
        /// </summary>
        int Order { get; }
    }
}
