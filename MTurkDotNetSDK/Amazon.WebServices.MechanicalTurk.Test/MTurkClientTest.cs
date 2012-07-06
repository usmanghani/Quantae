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
    /// Tests related to the core client
    /// </summary>
    [TestFixture]
    public class MTurkClientTest
    {
        private MTurkClient client;
        private Mockery mocks;
        private ITransportProtocol transport;

        [SetUp]
        public void setup()
        {
            mocks = new Mockery();
            transport = mocks.NewMock<ITransportProtocol>();
            client = new MTurkClient();
            client.Transport = transport;
            client.Config.MaxRetryDelay = 100; // 100ms maximum retry delay
        }

        [Test, Description("Sends a single request without a request envelope")]
        public void SendSingleRequestWithoutEnvelope()
        {
            ExpectAccountBalanceRequest();
            client.SendRequest(CreateAccountBalanceRequest());
            mocks.VerifyAllExpectationsHaveBeenMet();
        }

        [Test, Description("Sends multiple requests without a request envelope")]
        public void SendBatchRequestWithoutEnvelope()
        {
            ExpectAccountBalanceRequest(3);
            GetAccountBalanceRequest[] reqs = new GetAccountBalanceRequest[3];
            for (int i = 0; i < reqs.Length; i++)
            {
                reqs[i] = CreateAccountBalanceRequest();
            }

            client.SendRequest(reqs);
            mocks.VerifyAllExpectationsHaveBeenMet();
        }

        [Test, Description("Sends a single request within a request envelope")]
        public void SendRequestWithEnvelope()
        {
            ExpectAccountBalanceRequest();
            GetAccountBalance env = new GetAccountBalance();
            env.Request = new GetAccountBalanceRequest[] { CreateAccountBalanceRequest() };
            client.SendRequest(env);
            mocks.VerifyAllExpectationsHaveBeenMet();
       }

        [Test, Description("Sends single requests in multiple threads")]
        public void SendRequestsMultithreaded()
        {
            ExpectAccountBalanceRequest(1, 10);
            Thread[] threads = new Thread[int.Parse(client.Config.GetConfig("MTurkClientTest.SendRequestsMultithreaded.NumThreads", "10"))];
            for (int i = 0; i < threads.Length; i++)
            {
                threads[i] = new Thread(new ThreadStart(SendRequestMultithreaded));
                threads[i].Name = "GetBalanceThread" + i.ToString();
            }

            for (int i = 0; i < threads.Length; i++)
            {
                threads[i].Start();
            }

            // join threads
            for (int i = 0; i < threads.Length; i++)
            {
                threads[i].Join();
            }
            mocks.VerifyAllExpectationsHaveBeenMet();
        }

        [Test, Description("Sends a null request")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SendNullRequest()
        {
            client.SendRequest(null);
        }

        [Test, Description("Sends a request with invalid AWS credentials")]
        [ExpectedException(typeof(AccessKeyException))]
        public void SendRequestWithInvalidCredentials()
        {
            client.Config.SecretAccessKey = "Invalid";
            client.Transport = new SoapHttpClientTurkProtocol(client.Config.ServiceEndpoint, client.Config.Proxy);
            client.SendRequest(CreateAccountBalanceRequest());
        }

        [Test, Description("Sends a request to an invalid URL")]
        [ExpectedException(typeof(System.Net.WebException))]
        public void SendRequestToInvalidURL()
        {
            client.Config.ServiceEndpoint = "https://automaticturk.amazonaws.com/?Service=AWSMechanicalTurkRequester";
            client.Transport = new SoapHttpClientTurkProtocol(client.Config.ServiceEndpoint, client.Config.Proxy);
            client.SendRequest(CreateAccountBalanceRequest());
        }

        [Test, Description("Sends a request to an invalid Mechanical Turk host endpoint, but valid URL")]
        [ExpectedException(typeof(System.Net.WebException))]
        public void SendRequestToInvalidServiceHost()
        {
            client.Config.ServiceEndpoint = "https://www.amazon.com/?Service=AWSMechanicalTurkRequester";
            client.Transport = new SoapHttpClientTurkProtocol(client.Config.ServiceEndpoint, client.Config.Proxy);
            client.SendRequest(CreateAccountBalanceRequest());
        }

        [Test, Description("Sends a request to an invalid Mechanical Turk service endpoint, but valid URL")]
        [ExpectedException(typeof(ServiceException))]
        public void SendRequestToInvalidServiceEndpoint()
        {
            client.Config.ServiceEndpoint = "https://mechanicalturk.amazonaws.com/?Service=AWSMechanicalBritRequester";
            client.Transport = new SoapHttpClientTurkProtocol(client.Config.ServiceEndpoint, client.Config.Proxy);
            client.SendRequest(CreateAccountBalanceRequest());
        }

        [Test, Description("Sets a null configuration object")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void InstantiateClientWithNullConfiguration()
        {
            MTurkClient client = new MTurkClient(null);
        }

        [Test, Description("Sets a configuration without AWS access ID")]
        public void CheckThrottlerUsed()
        {
            IRequestThrottler throttler = (IRequestThrottler)mocks.NewMock(typeof(IRequestThrottler));
            client.Throttler = throttler;
            Expect.Once.On(throttler).Method("StartRequest");
            ExpectAccountBalanceRequest();
            client.SendRequest(CreateAccountBalanceRequest());
            mocks.VerifyAllExpectationsHaveBeenMet();
        }

        private GetAccountBalanceRequest CreateAccountBalanceRequest()
        {
            return new GetAccountBalanceRequest();
        }

        private void ExpectAccountBalanceRequest()
        {
            ExpectAccountBalanceRequest(CreateAccountBalanceResponse(), 1, 1);
        }

        private void ExpectAccountBalanceRequest(int numSubrequests)
        {
            ExpectAccountBalanceRequest(CreateAccountBalanceResponse(), numSubrequests, 1);
        }

        private void ExpectAccountBalanceRequest(int numSubrequests, int numRequests)
        {
            ExpectAccountBalanceRequest(CreateAccountBalanceResponse(), numSubrequests, numRequests);
        }

        private void ExpectAccountBalanceRequest(object response, int numSubrequests, int numRequests)
        {
            Expect.Exactly(numRequests).On(transport)
                .Method("DoTransport")
                .With(new GetAccountBalanceMatcher(numSubrequests), typeof(GetAccountBalance).Name)
                .Will(Return.Value(response));
        }

        private GetAccountBalanceResponse CreateAccountBalanceResponse()
        {
            GetAccountBalanceResponse response = new GetAccountBalanceResponse();
            response.OperationRequest = new OperationRequest();
            GetAccountBalanceResult result = new GetAccountBalanceResult();
            response.GetAccountBalanceResult = new GetAccountBalanceResult[] { result };
            result.Request = new Request();
            return response;
        }

        private void SendRequestMultithreaded()
        {
            client.SendRequest(CreateAccountBalanceRequest());
        }

        internal class GetAccountBalanceMatcher : Matcher
        {
            private int _count;

            public GetAccountBalanceMatcher(int count)
            {
                _count = count;
            }

            public override bool Matches(object o)
            {
                if (o.GetType().Equals(typeof(GetAccountBalance)))
                {
                    GetAccountBalance b = (GetAccountBalance)o;
                    GetAccountBalanceRequest[] req = b.Request;
                    return _count == req.Length;
                }
                return false;
            }

            public override void DescribeTo(TextWriter writer)
            {
                writer.Write("Expected a GetAccountBalance request envelope containing " + _count + " GetAccountBalanceRequest subrequests");
            }
        }
    }
}
