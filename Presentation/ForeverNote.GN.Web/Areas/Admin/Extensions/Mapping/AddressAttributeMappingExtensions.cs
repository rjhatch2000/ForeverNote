using ForeverNote.Core.Domain.Blogs;
using ForeverNote.Core.Domain.Catalog;
using ForeverNote.Core.Domain.Common;
using ForeverNote.Core.Domain.Courses;
using ForeverNote.Core.Domain.Customers;
using ForeverNote.Core.Domain.Directory;
using ForeverNote.Core.Domain.Discounts;
using ForeverNote.Core.Domain.Documents;
using ForeverNote.Core.Domain.Forums;
using ForeverNote.Core.Domain.Knowledgebase;
using ForeverNote.Core.Domain.Localization;
using ForeverNote.Core.Domain.Logging;
using ForeverNote.Core.Domain.Media;
using ForeverNote.Core.Domain.Messages;
using ForeverNote.Core.Domain.News;
using ForeverNote.Core.Domain.Orders;
using ForeverNote.Core.Domain.Polls;
using ForeverNote.Core.Domain.Shipping;
using ForeverNote.Core.Domain.Stores;
using ForeverNote.Core.Domain.Tax;
using ForeverNote.Core.Domain.Topics;
using ForeverNote.Core.Domain.Vendors;
using ForeverNote.Core.Infrastructure.Mapper;
using ForeverNote.Core.Plugins;
using ForeverNote.Services.Authentication.External;
using ForeverNote.Services.Cms;
using ForeverNote.Services.Common;
using ForeverNote.Services.Directory;
using ForeverNote.Services.Helpers;
using ForeverNote.Services.Payments;
using ForeverNote.Services.Shipping;
using ForeverNote.Services.Tax;
using ForeverNote.Web.Areas.Admin.Models.Blogs;
using ForeverNote.Web.Areas.Admin.Models.Catalog;
using ForeverNote.Web.Areas.Admin.Models.Cms;
using ForeverNote.Web.Areas.Admin.Models.Common;
using ForeverNote.Web.Areas.Admin.Models.Courses;
using ForeverNote.Web.Areas.Admin.Models.Customers;
using ForeverNote.Web.Areas.Admin.Models.Directory;
using ForeverNote.Web.Areas.Admin.Models.Discounts;
using ForeverNote.Web.Areas.Admin.Models.Documents;
using ForeverNote.Web.Areas.Admin.Models.ExternalAuthentication;
using ForeverNote.Web.Areas.Admin.Models.Forums;
using ForeverNote.Web.Areas.Admin.Models.Knowledgebase;
using ForeverNote.Web.Areas.Admin.Models.Localization;
using ForeverNote.Web.Areas.Admin.Models.Logging;
using ForeverNote.Web.Areas.Admin.Models.Messages;
using ForeverNote.Web.Areas.Admin.Models.News;
using ForeverNote.Web.Areas.Admin.Models.Orders;
using ForeverNote.Web.Areas.Admin.Models.Payments;
using ForeverNote.Web.Areas.Admin.Models.Plugins;
using ForeverNote.Web.Areas.Admin.Models.Polls;
using ForeverNote.Web.Areas.Admin.Models.Settings;
using ForeverNote.Web.Areas.Admin.Models.Shipping;
using ForeverNote.Web.Areas.Admin.Models.Stores;
using ForeverNote.Web.Areas.Admin.Models.Tax;
using ForeverNote.Web.Areas.Admin.Models.Templates;
using ForeverNote.Web.Areas.Admin.Models.Topics;
using ForeverNote.Web.Areas.Admin.Models.Vendors;
using System;
using System.Threading.Tasks;

namespace ForeverNote.Web.Areas.Admin.Extensions
{
    public static class AddressAttributeMappingExtensions
    {
        //attributes
        public static AddressAttributeModel ToModel(this AddressAttribute entity)
        {
            return entity.MapTo<AddressAttribute, AddressAttributeModel>();
        }

        public static AddressAttribute ToEntity(this AddressAttributeModel model)
        {
            return model.MapTo<AddressAttributeModel, AddressAttribute>();
        }

        public static AddressAttribute ToEntity(this AddressAttributeModel model, AddressAttribute destination)
        {
            return model.MapTo(destination);
        }

        //attributes value
        public static AddressAttributeValueModel ToModel(this AddressAttributeValue entity)
        {
            return entity.MapTo<AddressAttributeValue, AddressAttributeValueModel>();
        }
        public static AddressAttributeValue ToEntity(this AddressAttributeValueModel model)
        {
            return model.MapTo<AddressAttributeValueModel, AddressAttributeValue>();
        }

        public static AddressAttributeValue ToEntity(this AddressAttributeValueModel model, AddressAttributeValue destination)
        {
            return model.MapTo(destination);
        }
    }
}