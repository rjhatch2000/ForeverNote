using DotLiquid;
using ForeverNote.Core.Domain.Common;
using ForeverNote.Core.Domain.Localization;
using ForeverNote.Core.Domain.Notes;
using ForeverNote.Services.Localization;
using System.Collections.Generic;

namespace ForeverNote.Services.Messages.DotLiquidDrops
{
    public partial class LiquidNote : Drop
    {
        private readonly CommonSettings _commonSettings;
        private readonly Note _note;
        private readonly Language _language;

        public LiquidNote(
            CommonSettings commonSettings,
            Note note,
            Language language
        )
        {
            _commonSettings = commonSettings;
            _note = note;
            _language = language;
            AdditionalTokens = new Dictionary<string, string>();
        }

        public string Id
        {
            get { return _note.Id.ToString(); }
        }

        public string Name
        {
            get { return _note.GetLocalized(x => x.Name, _language.Id); }
        }

        public string ShortDescription
        {
            get { return _note.GetLocalized(x => x.ShortDescription, _language.Id); }
        }

        public string NoteURLForUser
        {
            get
            {
                return string.Format("{0}{1}{2}",
                    _commonSettings.SslEnabled ? _commonSettings.SecureUrl : _commonSettings.Url,
                    "/note/",
                    _note.Id
                );
            }
        }

        public IDictionary<string, string> AdditionalTokens { get; set; }
    }
}