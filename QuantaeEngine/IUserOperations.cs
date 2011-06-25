using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Quantae.Engine
{
    public interface IUserOperations
    {
        bool RegisterUser(string username, string email, string passwordHash);
    }
}
