using ForeverNote.Core;
using ForeverNote.Core.Data;
using ForeverNote.Core.Domain.Common;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ForeverNote.Services.Common
{
    /// <summary>
    /// User field service
    /// </summary>
    public class GenericAttributeService : IGenericAttributeService
    {

        #region Fields

        private readonly IRepository<GenericAttributeBaseEntity> _genericAttributeBaseEntityRepository;
        #endregion

        #region Ctor
        public GenericAttributeService(
            IRepository<GenericAttributeBaseEntity> genericAttributeBaseEntityRepository)
        {
            _genericAttributeBaseEntityRepository = genericAttributeBaseEntityRepository;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Save attribute value
        /// </summary>
        /// <typeparam name="TPropType">Property type</typeparam>
        /// <param name="entity">Entity</param>
        /// <param name="key">Key</param>
        /// <param name="value">Value</param>
        /// <param name="storeId">Store identifier; pass "" if this attribute will be available for all stores</param>
        public virtual async Task SaveField<TPropType>(BaseEntity entity, string key, TPropType value)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            if (key == null)
                throw new ArgumentNullException(nameof(key));

            var collectionName = entity.GetType().Name;

            _ = _genericAttributeBaseEntityRepository.SetCollection(collectionName);

            var baseFields = await _genericAttributeBaseEntityRepository.GetByIdAsync(entity.Id);

            var props = baseFields.GenericAttributes;

            var prop = props.FirstOrDefault(ga =>
                ga.Key.Equals(key, StringComparison.OrdinalIgnoreCase)); //should be culture invariant

            var valueStr = CommonHelper.To<string>(value);

            if (prop != null)
            {
                if (string.IsNullOrWhiteSpace(valueStr))
                {
                    //delete
                    await _genericAttributeBaseEntityRepository.PullFilter(entity.Id, x => x.GenericAttributes, y => y.Key == prop.Key);

                    var entityProp = entity.GenericAttributes.FirstOrDefault(x => x.Key == prop.Key);
                    if (entityProp != null)
                        entity.GenericAttributes.Remove(entityProp);
                }
                else
                {
                    //update
                    prop.Value = valueStr;
                    await _genericAttributeBaseEntityRepository.UpdateToSet(entity.Id, x => x.GenericAttributes, y => y.Key == prop.Key, prop);

                    var entityProp = entity.GenericAttributes.FirstOrDefault(x => x.Key == prop.Key);
                    if (entityProp != null)
                        entityProp.Value = valueStr;

                }
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(valueStr))
                {
                    prop = new GenericAttribute {
                        Key = key,
                        Value = valueStr,
                    };
                    await _genericAttributeBaseEntityRepository.AddToSet(entity.Id, x => x.GenericAttributes, prop);

                    entity.GenericAttributes.Add(prop);
                }
            }
        }

        public virtual async Task<TPropType> GetFieldsForEntity<TPropType>(BaseEntity entity, string key)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            var collectionName = entity.GetType().Name;
            _ = _genericAttributeBaseEntityRepository.SetCollection(collectionName);

            var baseFields = await _genericAttributeBaseEntityRepository.GetByIdAsync(entity.Id);
            var props = baseFields?.GenericAttributes;
            if (props == null)
                return default;
            props = props.ToList();
            if (!props.Any())
                return default;

            var prop = props.FirstOrDefault(ga =>
                ga.Key.Equals(key, StringComparison.OrdinalIgnoreCase));

            if (prop == null || string.IsNullOrEmpty(prop.Value))
                return default;

            return CommonHelper.To<TPropType>(prop.Value);
        }

        #endregion
    }
}