using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Quantae.Engine
{
    public enum CreateUserFailureReason { Unknown = 0, AlreadyExists = 1, EmailInUse = 2, }
    
    public class CreateUserResult
    {
        public bool Success { get; set; }
        public string Token { get; set; }
        public CreateUserFailureReason Reason { get; set; }
    }
}
