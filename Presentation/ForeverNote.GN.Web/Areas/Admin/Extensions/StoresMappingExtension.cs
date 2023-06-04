﻿using ForeverNote.Core.Domain.Stores;
using ForeverNote.Web.Framework.Mapping;
using ForeverNote.Web.Framework.Mvc.Models;
using ForeverNote.Services.Stores;
using System.Linq;
using System.Threading.Tasks;

namespace ForeverNote.Web.Areas.Admin.Extensions
{
    public static class StoresMappingExtension
    {
        public static async Task PrepareStoresMappingModel<T>(this T baseGrandEntityModel, IStoreMappingSupported storeMapping, IStoreService _storeService, bool excludeProperties, string storeId = null) 
            where T : BaseForeverNoteEntityModel, IStoreMappingModel
        {
            baseGrandEntityModel.AvailableStores = (await _storeService
               .GetAllStores()).Where(x=>x.Id == storeId || string.IsNullOrEmpty(storeId))
               .Select(s => new StoreModel { Id = s.Id, Name = s.Shortcut })
               .ToList();
            if (!excludeProperties)
            {
                if (storeMapping != null)
                {
                    baseGrandEntityModel.SelectedStoreIds = storeMapping.Stores.ToArray();
                }
            }
            if (!string.IsNullOrEmpty(storeId))
            {
                baseGrandEntityModel.LimitedToStores = true;
                baseGrandEntityModel.SelectedStoreIds = new string[] { storeId };
            }
        }       
    }
}
