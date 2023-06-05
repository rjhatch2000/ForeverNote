using ForeverNote.Core;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ForeverNote.Services.Common
{
    public static class GenericAttributeExtensions
    {
        /// <summary>
        /// Get an attribute of an entity
        /// </summary>
        /// <typeparam name="TPropType">Property type</typeparam>
        /// <param name="entity">Entity</param>
        /// <param name="genericAttributeService">GenericAttributeService</param>
        /// <param name="key">Key</param>
        /// <returns>Attribute</returns>
        public static async Task<TPropType> GetAttribute<TPropType>(this BaseEntity entity, IGenericAttributeService genericAttributeService, string key)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            return await genericAttributeService.GetAttributesForEntity<TPropType>(entity, key);
        }

        /// <summary>
        /// Get an attribute of an entity
        /// </summary>
        /// <typeparam name="TPropType">Property type</typeparam>
        /// <param name="entity">Entity</param>
        /// <param name="key">Key</param>
        /// <returns>Attribute</returns>
        public static TPropType GetAttributeFromEntity<TPropType>(this BaseEntity entity, string key)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            var props = entity.GenericAttributes;
            if (props == null)
                return default(TPropType);
            if (!props.Any())
                return default(TPropType);

            var prop = props.FirstOrDefault(ga =>
                ga.Key.Equals(key, StringComparison.InvariantCultureIgnoreCase));

            if (prop == null || string.IsNullOrEmpty(prop.Value))
                return default(TPropType);

            return CommonHelper.To<TPropType>(prop.Value);
        }

    }
}
