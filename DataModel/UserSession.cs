﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Quantae.DataModel
{
    public class UserSession : QuantaeObject<long>
    {
        public string Token { get; set; }
        public DateTime Timestamp { get; set; }
        public string UserID { get; set; }
        public UserProfileHandle UserProfile { get; set; }
    }

    public class UserSessionHandle : QuantaeObjectHandle<long, UserSession>
    {
        public UserSessionHandle(UserSession session) : base(session) { }
    }
}
