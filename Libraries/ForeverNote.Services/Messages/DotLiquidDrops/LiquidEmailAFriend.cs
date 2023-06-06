using DotLiquid;
using System.Collections.Generic;

namespace ForeverNote.Services.Messages.DotLiquidDrops
{
    public partial class LiquidEmailAFriend : Drop
    {
        private string _personalMessage;
        private string _userEmail;

        public LiquidEmailAFriend(string personalMessage, string userEmail)
        {
            this._personalMessage = personalMessage;
            this._userEmail = userEmail;

            AdditionalTokens = new Dictionary<string, string>();
        }

        public string PersonalMessage
        {
            get { return _personalMessage; }
        }

        public string Email
        {
            get { return _userEmail; }
        }

        public IDictionary<string, string> AdditionalTokens { get; set; }
    }
}
