﻿using System.Collections.Generic;

namespace ForeverNote.Services.Authentication
{
    public partial class TwoFactorCodeSetup
    {
        public TwoFactorCodeSetup()
        {
            CustomValues = new Dictionary<string, string>();
        }
        public IDictionary<string, string> CustomValues { get; set; }
    }
}
