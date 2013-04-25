#region Copyright & license notice
/*
 * Copyright: Copyright (c) 2007 Amazon Technologies, Inc.
 * License:   Apache License, Version 2.0
 */
#endregion

using System;
using System.IO;
using System.Threading;
using NUnit.Framework;
using Amazon.WebServices.MechanicalTurk.Domain;
using Amazon.WebServices.MechanicalTurk.Exceptions;
using Amazon.WebServices.MechanicalTurk.Advanced;


namespace Amazon.WebServices.MechanicalTurk.Test
{
    /// <summary>
    /// Tests the ILog feature in the SDK
    /// </summary>
    [TestFixture]
    public class LogTest
    {
        [Test, Description("Sets a null logger for the SDK")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SetNullLogger()
        {
            MTurkLog.Logger = null;
        }

        [Test, Description("Sets a string logger and tests whether it succeeded")]
        public void SetStringLogger()
        {
            MTurkLog.Level = 1;
            StringTestLogger newLogger = new StringTestLogger();
            MTurkLog.Logger = newLogger;
            TestUtil.CreateSingleHit("Test", "Logger test", "no question,right?", 1);

            string log = newLogger.LogOutput;            
            Assert.IsTrue(log.Length > 0, "No log messages received from SDK logger");
            Assert.IsTrue(log.IndexOf("</soap:Envelope>") != -1, "No SOAP trace received from SDK logger");
        }

        [Test, Description("Sets an invalid log level")]
        [ExpectedException(typeof(ArgumentException))]
        public void SetInvalidLogLevel()
        {
            MTurkLog.Level = 13;
        }

        [Test, Description("Sets a valid log level")]
        public void SetValidLogLevel()
        {
            MTurkLog.Level = 5;
            MTurkLog.Level = 4;
            MTurkLog.Level = 3;
            MTurkLog.Level = 2;
            MTurkLog.Level = 1;
        }


    }

    public class StringTestLogger : ILog
    {
        private System.Text.StringBuilder sb = new System.Text.StringBuilder();
        public string LogOutput
        {
            get
            {
                return sb.ToString();
            }
        }

        #region ILog Members

        public void Debug(string format, params object[] args)
        {
            sb.Append("DEBUG ");
            sb.AppendFormat(format, args);
            sb.AppendLine();
        }

        public void Error(string format, params object[] args)
        {
            sb.Append("DEBUG ");
            sb.AppendFormat(format, args);
            sb.AppendLine();
        }

        public void Info(string format, params object[] args)
        {
            sb.Append("DEBUG ");
            sb.AppendFormat(format, args);
            sb.AppendLine();
        }

        public void Warn(string format, params object[] args)
        {
            sb.Append("DEBUG ");
            sb.AppendFormat(format, args);
            sb.AppendLine();
        }

        #endregion
    }
}
