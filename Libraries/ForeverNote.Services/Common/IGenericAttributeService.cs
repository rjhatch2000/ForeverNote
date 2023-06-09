using ForeverNote.Core;
using System.Threading.Tasks;

namespace ForeverNote.Services.Common
{
    /// <summary>
    /// Generic attribute service interface
    /// </summary>
    public partial interface IGenericAttributeService
    {
        Task SaveAttribute<TPropType>(BaseEntity entity, string key, TPropType value);
        Task SaveAttribute<TPropType>(string entity, string entityId, string key, TPropType value);
        Task<TPropType> GetAttributesForEntity<TPropType>(BaseEntity entity, string key);
    }
}