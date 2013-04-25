using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using log4net;
using System.Reflection;

namespace QuantaeWebApp
{
    public interface ILoggingService
    {
        ILog Logger { get; }
    }

    public class LoggingService : ILoggingService
    {
        private static readonly ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public ILog Logger
        {
            get { return logger; }
        }
    }
}