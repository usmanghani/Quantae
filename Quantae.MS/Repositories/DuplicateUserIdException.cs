using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Quantae.Repositories
{
    class DuplicateUserIdException : Exception
    {
        public DuplicateUserIdException(string userId) : base(string.Format("Duplicate user id {0}", userId)) { }
    }
}
