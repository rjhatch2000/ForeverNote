using DotLiquid;
using System.Collections.Generic;

namespace ForeverNote.Core.Domain.Messages
{
    /// <summary>
    /// An object that acumulates all DotLiquid Drops
    /// </summary>
    public partial class LiquidObject
    {
        public LiquidObject()
        {
            AdditionalTokens = new Dictionary<string, string>();
        }

        public Drop AttributeCombination { get; set; }

        public Drop User { get; set; }

        public Drop Note { get; set; }

        //TODO: Delete?
        public Drop EmailAFriend { get; set; }

        //TODO: Delete?
        public Drop AskQuestion { get; set; }

        public Drop ContactUs { get; set; }

        public IDictionary<string, string> AdditionalTokens { get; set; }
    }
}