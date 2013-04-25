#region Copyright & license notice
/*
 * Copyright: Copyright (c) 2007 Amazon Technologies, Inc.
 * License:   Apache License, Version 2.0
 */
#endregion

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.IO;
using NUnit.Framework;
using NMock2;
using NMock2.Matchers;
using Amazon.WebServices.MechanicalTurk.Domain;
using Amazon.WebServices.MechanicalTurk.Exceptions;
using Amazon.WebServices.MechanicalTurk.Advanced;

namespace Amazon.WebServices.MechanicalTurk.Test
{
    /// <summary>
    /// Tests related to the client config
    /// </summary>
    [TestFixture]
    public class MTurkConfigTest
    {
        private MTurkConfig config;

        [SetUp]
        public void createConfig()
        {
            config = new MTurkConfig();
        }

        [Test, Description("Sets a configuration without AWS access ID")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void InstantiateInvalidConfiguration()
        {
            config.AccessKeyId = null;
        }

        [Test, Description("Reads a valid/existing configuration value")]
        public void ReadExistingConfigurationKey()
        {
            Assert.IsNotNull(config.GetConfig("MechanicalTurk.AccessKeyId", null));
        }

        [Test, Description("Reads a non-existing configuration value")]
        public void ReadNonExistingConfigurationKey()
        {
            Assert.IsNull(config.GetConfig("MechanicalTurk.Bogus", null));
        }

        [Test, Description("Reads a configuration value for a null key")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ReadNullConfigurationKey()
        {
            config.GetConfig(null, null);
        }

        [Test, Description("Mimicks external configuration (non app.config)")]
        public void TestExternalConfig()
        {
            string awsUrl = "http://www.example.com";
            string awsId = "some id";
            string awsPw = "some key";

            config = new MTurkConfig(awsUrl, awsId, awsPw);

            Assert.AreEqual("http://www.example.com", config.ServiceEndpoint);
            Assert.AreEqual("some id", config.AccessKeyId);
            Assert.AreEqual("some key", config.SecretAccessKey);
        }
    }
}
