using ForeverNote.Core.Domain.Catalog;
using ForeverNote.Core.Domain.Customers;
using ForeverNote.Core.Domain.Documents;
using ForeverNote.Core.Domain.Logging;
using ForeverNote.Core.Domain.Media;
using ForeverNote.Core.Domain.Messages;
using ForeverNote.Core.Domain.Orders;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Bson.Serialization.Serializers;
using System;

namespace ForeverNote.Core.Infrastructure.MongoDB
{
    public static class MongoDBMapperConfiguration
    {

        /// <summary>
        /// Register MongoDB mappings
        /// </summary>
        public static void RegisterMongoDBMappings()
        {
            BsonSerializer.RegisterSerializer(typeof(decimal), new DecimalSerializer(BsonType.Decimal128));
            BsonSerializer.RegisterSerializer(typeof(decimal?), new NullableSerializer<decimal>(new DecimalSerializer(BsonType.Decimal128)));
            BsonSerializer.RegisterSerializer(typeof(DateTime), new BsonUtcDateTimeSerializer());

            //global set an equivalent of [BsonIgnoreExtraElements] for every Domain Model
            var cp = new ConventionPack();
            cp.Add(new IgnoreExtraElementsConvention(true));
            ConventionRegistry.Register("ApplicationConventions", cp, t => true);

            RegisterClassProduct();
            RegisterClassProductCategory();
            RegisterClassProductPicture();
            RegisterClassProductTag();
            RegisterClassCustomer();
            RegisterClassCustomerAction();
            RegisterClassActionCondition();
            RegisterClassCustomerAttribute();
            RegisterClassCustomerHistoryPassword();
            RegisterClassCustomerReminder();
            RegisterClassReminderCondition();
            RegisterClassCustomerReminderHistory();
            RegisterClassCustomerRole();
            RegisterClassLog();
            RegisterClassDownload();
            RegisterClassCampaign();
            RegisterClassEmailAccount();
            RegisterClassFormAttribute();
            RegisterClassMessageTemplate();
            RegisterClassQueuedEmail();
            RegisterClassDocument();
        }

        private static void RegisterClassProduct()
        {
            BsonClassMap.RegisterClassMap<Product>(cm =>
            {
                cm.AutoMap();
                //ignore these Fields, an equivalent of [BsonIgnore]
                cm.UnmapMember(c => c.IntervalUnitType);
                cm.UnmapMember(c => c.RecurringCyclePeriod);
            });
        }
        private static void RegisterClassProductCategory()
        {
            BsonClassMap.RegisterClassMap<ProductCategory>(cm =>
            {
                cm.AutoMap();
                cm.UnmapMember(c => c.ProductId);
            });
        }
        private static void RegisterClassProductPicture()
        {
            BsonClassMap.RegisterClassMap<ProductPicture>(cm =>
            {
                cm.AutoMap();
                cm.UnmapMember(c => c.ProductId);
            });

        }
        private static void RegisterClassProductTag()
        {
            BsonClassMap.RegisterClassMap<ProductTag>(cm =>
            {
                cm.AutoMap();
                cm.UnmapMember(c => c.ProductId);
            });
        }
        private static void RegisterClassCustomer()
        {
            BsonClassMap.RegisterClassMap<Customer>(cm =>
            {
                cm.AutoMap();
                cm.UnmapMember(c => c.PasswordFormat);
            });
        }
        private static void RegisterClassCustomerAction()
        {
            BsonClassMap.RegisterClassMap<CustomerAction>(cm =>
            {
                cm.AutoMap();
                cm.UnmapMember(c => c.Condition);
                cm.UnmapMember(c => c.ReactionType);
            });
        }
        private static void RegisterClassActionCondition()
        {
            BsonClassMap.RegisterClassMap<CustomerAction.ActionCondition>(cm =>
            {
                cm.AutoMap();
                cm.UnmapMember(c => c.CustomerActionConditionType);
                cm.UnmapMember(c => c.Condition);
            });
        }
        private static void RegisterClassCustomerAttribute()
        {
            BsonClassMap.RegisterClassMap<CustomerAttribute>(cm =>
            {
                cm.AutoMap();
            });
        }
        private static void RegisterClassCustomerHistoryPassword()
        {
            BsonClassMap.RegisterClassMap<CustomerHistoryPassword>(cm =>
            {
                cm.AutoMap();
                cm.UnmapMember(c => c.PasswordFormat);
            });
        }
        private static void RegisterClassCustomerReminder()
        {
            BsonClassMap.RegisterClassMap<CustomerReminder>(cm =>
            {
                cm.AutoMap();
                cm.UnmapMember(c => c.Condition);
                cm.UnmapMember(c => c.ReminderRule);
            });
        }
        private static void RegisterClassReminderCondition()
        {
            BsonClassMap.RegisterClassMap<CustomerReminder.ReminderCondition>(cm =>
            {
                cm.AutoMap();
                cm.UnmapMember(c => c.ConditionType);
                cm.UnmapMember(c => c.Condition);
            });
        }
        private static void RegisterClassCustomerReminderHistory()
        {
            BsonClassMap.RegisterClassMap<CustomerReminderHistory>(cm =>
            {
                cm.AutoMap();
                cm.UnmapMember(c => c.ReminderRule);
                cm.UnmapMember(c => c.HistoryStatus);
            });
        }
        private static void RegisterClassCustomerRole()
        {
            BsonClassMap.RegisterClassMap<CustomerRole>(cm =>
            {
                cm.AutoMap();
                cm.UnmapMember(c => c.CustomerId);
            });
        }
        private static void RegisterClassLog()
        {
            BsonClassMap.RegisterClassMap<Log>(cm =>
            {
                cm.AutoMap();
                cm.UnmapMember(c => c.LogLevel);
            });
        }
        private static void RegisterClassDownload()
        {
            BsonClassMap.RegisterClassMap<Download>(cm =>
            {
                cm.AutoMap();
                cm.UnmapMember(c => c.DownloadBinary);
            });
        }
        private static void RegisterClassCampaign()
        {
            BsonClassMap.RegisterClassMap<Campaign>(cm =>
            {
                cm.AutoMap();
            });
        }
        private static void RegisterClassEmailAccount()
        {
            BsonClassMap.RegisterClassMap<EmailAccount>(cm =>
            {
                cm.AutoMap();
                cm.UnmapMember(c => c.FriendlyName);
            });
        }
        private static void RegisterClassFormAttribute()
        {
            BsonClassMap.RegisterClassMap<InteractiveForm.FormAttribute>(cm =>
            {
                cm.AutoMap();
                cm.UnmapMember(c => c.AttributeControlType);
            });
        }
        private static void RegisterClassMessageTemplate()
        {
            BsonClassMap.RegisterClassMap<MessageTemplate>(cm =>
            {
                cm.AutoMap();
                cm.UnmapMember(c => c.DelayPeriod);
            });
        }
        private static void RegisterClassQueuedEmail()
        {
            BsonClassMap.RegisterClassMap<QueuedEmail>(cm =>
            {
                cm.AutoMap();
                cm.UnmapMember(c => c.Priority);
            });
        }
        private static void RegisterClassDocument()
        {
            BsonClassMap.RegisterClassMap<Document>(cm =>
            {
                cm.AutoMap();
                cm.UnmapMember(c => c.DocumentStatus);
                cm.UnmapMember(c => c.Reference);
            });
        }
    }
}
