using DotLiquid;
using ForeverNote.Core.Domain.Common;
using ForeverNote.Core.Domain.Notes;
using System.Collections.Generic;

namespace ForeverNote.Services.Messages.DotLiquidDrops
{
    public partial class LiquidBackInStockSubscription : Drop
    {

        private readonly CommonSettings _commonSettings;
        private readonly Note _note;

        public LiquidBackInStockSubscription(
            CommonSettings commonSettings,
            Note note
        )
        {
            _commonSettings = commonSettings;
            _note = note;

            AdditionalTokens = new Dictionary<string, string>();
        }

        public string NoteName
        {
            get { return _note.Name; }
        }

        public string NoteUrl
        {
            get { return string.Format("{0}{1}{2}",
                _commonSettings.SslEnabled ? _commonSettings.SecureUrl : _commonSettings.Url,
                "/note/",
                _note.Id); }
        }

        public IDictionary<string, string> AdditionalTokens { get; set; }
    }
}