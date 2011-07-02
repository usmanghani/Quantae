using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Quantae.Engine
{
    public class SessionNotFoundException : Exception
    {
        public SessionNotFoundException(string token) : base(string.Format("Session with token {0} not found.", token)) { }
    }
}
