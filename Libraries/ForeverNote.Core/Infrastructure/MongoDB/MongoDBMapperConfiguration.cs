using ForeverNote.Core.Domain.Logging;
using ForeverNote.Core.Domain.Media;
using ForeverNote.Core.Domain.Messages;
using ForeverNote.Core.Domain.Notes;
using ForeverNote.Core.Domain.Users;
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

            RegisterClassNote();
            RegisterClassNoteNotebook();
            RegisterClassNotePicture();
            RegisterClassNoteTag();
            RegisterClassUser();
            RegisterClassUserAction();
            RegisterClassActionCondition();
            RegisterClassUserAttribute();
            RegisterClassUserHistoryPassword();
            RegisterClassUserReminder();
            RegisterClassReminderCondition();
            RegisterClassUserReminderHistory();
            RegisterClassLog();
            RegisterClassDownload();
            RegisterClassCampaign();
            RegisterClassEmailAccount();
            RegisterClassFormAttribute();
            RegisterClassMessageTemplate();
            RegisterClassQueuedEmail();
        }

        private static void RegisterClassNote()
        {
            BsonClassMap.RegisterClassMap<Note>(cm =>
            {
                cm.AutoMap();
                //ignore these Fields, an equivalent of [BsonIgnore]
                cm.UnmapMember(c => c.IntervalUnitType);
                cm.UnmapMember(c => c.RecurringCyclePeriod);
            });
        }
        private static void RegisterClassNoteNotebook()
        {
            BsonClassMap.RegisterClassMap<NoteNotebook>(cm =>
            {
                cm.AutoMap();
                cm.UnmapMember(c => c.NoteId);
            });
        }
        private static void RegisterClassNotePicture()
        {
            BsonClassMap.RegisterClassMap<NotePicture>(cm =>
            {
                cm.AutoMap();
                cm.UnmapMember(c => c.NoteId);
            });

        }
        private static void RegisterClassNoteTag()
        {
            BsonClassMap.RegisterClassMap<NoteTag>(cm =>
            {
                cm.AutoMap();
                cm.UnmapMember(c => c.NoteId);
            });
        }
        private static void RegisterClassUser()
        {
            BsonClassMap.RegisterClassMap<User>(cm =>
            {
                cm.AutoMap();
                cm.UnmapMember(c => c.PasswordFormat);
            });
        }
        private static void RegisterClassUserAction()
        {
            BsonClassMap.RegisterClassMap<UserAction>(cm =>
            {
                cm.AutoMap();
                cm.UnmapMember(c => c.Condition);
                cm.UnmapMember(c => c.ReactionType);
            });
        }
        private static void RegisterClassActionCondition()
        {
            BsonClassMap.RegisterClassMap<UserAction.ActionCondition>(cm =>
            {
                cm.AutoMap();
                cm.UnmapMember(c => c.UserActionConditionType);
                cm.UnmapMember(c => c.Condition);
            });
        }
        private static void RegisterClassUserAttribute()
        {
            BsonClassMap.RegisterClassMap<UserAttribute>(cm =>
            {
                cm.AutoMap();
            });
        }
        private static void RegisterClassUserHistoryPassword()
        {
            BsonClassMap.RegisterClassMap<UserHistoryPassword>(cm =>
            {
                cm.AutoMap();
                cm.UnmapMember(c => c.PasswordFormat);
            });
        }
        private static void RegisterClassUserReminder()
        {
            BsonClassMap.RegisterClassMap<UserReminder>(cm =>
            {
                cm.AutoMap();
                cm.UnmapMember(c => c.Condition);
                cm.UnmapMember(c => c.ReminderRule);
            });
        }
        private static void RegisterClassReminderCondition()
        {
            BsonClassMap.RegisterClassMap<UserReminder.ReminderCondition>(cm =>
            {
                cm.AutoMap();
                cm.UnmapMember(c => c.ConditionType);
                cm.UnmapMember(c => c.Condition);
            });
        }
        private static void RegisterClassUserReminderHistory()
        {
            BsonClassMap.RegisterClassMap<UserReminderHistory>(cm =>
            {
                cm.AutoMap();
                cm.UnmapMember(c => c.ReminderRule);
                cm.UnmapMember(c => c.HistoryStatus);
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
    }
}
