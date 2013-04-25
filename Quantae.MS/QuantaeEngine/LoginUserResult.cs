using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Quantae.Engine
{
    public enum LoginFailureReason { Unknown = 0, NotAUser = 1, InvalidPassword = 2 }

    public class LoginUserResult
    {
        public bool Success { get; set; }
        public string Token { get; set; }
        public LoginFailureReason Reason { get; set; }
    }
}
