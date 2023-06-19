using ForeverNote.Core;
using System.Threading.Tasks;

namespace ForeverNote.Services.Common
{
    /// <summary>
    /// User fields service interface
    /// </summary>
    public interface IGenericAttributeService
    {
        Task SaveField<TPropType>(BaseEntity entity, string key, TPropType value);
        Task<TPropType> GetFieldsForEntity<TPropType>(BaseEntity entity, string key);
    }
}