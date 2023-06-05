using DotLiquid;
using ForeverNote.Core.Domain.Common;
using ForeverNote.Core.Domain.Messages;
using System;
using System.Collections.Generic;

namespace ForeverNote.Services.Messages.DotLiquidDrops
{
    public partial class LiquidNewsLetterSubscription : Drop
    {
        private readonly CommonSettings _commonSettings;
        private readonly NewsLetterSubscription _subscription;

        public LiquidNewsLetterSubscription(
            CommonSettings commonSettings,
            NewsLetterSubscription subscription
        )
        {
            _commonSettings = commonSettings;
            _subscription = subscription;
            AdditionalTokens = new Dictionary<string, string>();
        }

        public string Email
        {
            get { return _subscription.Email; }
        }

        public string ActivationUrl
        {
            get
            {
                string urlFormat = "{0}newsletter/subscriptionactivation/{1}/{2}";
                var activationUrl = String.Format(urlFormat, (_commonSettings.SslEnabled ? _commonSettings.SecureUrl : _commonSettings.Url), _subscription.NewsLetterSubscriptionGuid, "true");
                return activationUrl;
            }
        }

        public string DeactivationUrl
        {
            get
            {
                string urlFormat = "{0}newsletter/subscriptionactivation/{1}/{2}";
                var deActivationUrl = String.Format(urlFormat, (_commonSettings.SslEnabled ? _commonSettings.SecureUrl : _commonSettings.Url), _subscription.NewsLetterSubscriptionGuid, "false");
                return deActivationUrl;
            }
        }

        public IDictionary<string, string> AdditionalTokens { get; set; }
    }
}