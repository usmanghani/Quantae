#region Copyright & license notice
/*
 * Copyright: Copyright (c) 2007 Amazon Technologies, Inc.
 * License:   Apache License, Version 2.0
 */
#endregion

using System;
using System.IO;
using System.Diagnostics;
using NUnit.Framework;
using System.Collections.Generic;
using Amazon.WebServices.MechanicalTurk.Domain;
using Amazon.WebServices.MechanicalTurk.Exceptions;
using Amazon.WebServices.MechanicalTurk.Advanced;



namespace Amazon.WebServices.MechanicalTurk.Test
{
    /// <summary>
    /// Hit operation tests
    /// </summary>
	[TestFixture]
	public class HitTest
	{
        [TestFixtureSetUp]
        public void CheckSandbox()
        {
            TestUtil.CheckSandbox();
        }

        /// <summary>
        /// Disposes HITs created during the test
        /// </summary>
        [TearDown]
        public void DisposeHITs()
        {
            if (TestUtil.CreatedHits.Count > 0)
            {
                foreach (string hitID in TestUtil.CreatedHits)
                {
                    try
                    {
                        TestUtil.ExpireHitByID(hitID);

                        DisposeHITRequest request = new DisposeHITRequest();
                        request.HITId = hitID;

                        TestUtil.Client.DisposeHIT(request);
                    }
                    catch
                    {
                    }
                }
            }
        }

		private Random rand = new Random();       				
        [Test, Description("Creates a simple hit")]
        public void CreateHit()
		{
			HIT h = TestUtil.CreateSingleHit("CreateHit", "This is a question created by the mturk .Net SDK", string.Format("What's the next prime number that is > {0}?", rand.Next(100, 50000)), rand.Next(5));

            TestUtil.CreatedHits.Add(h.HITId);
        }

        [Test, Description("Creates a hit by reading the QuestionForm from a question file")]
        public void CreateHitFromQuestionFormFile()
        {
            HIT h = TestUtil.CreateSingleHit("CreateHit", "Question created from HIT", Path.Combine(TestUtil.DataPath, "QuestionForm.xml"), rand.Next(5));

            TestUtil.CreatedHits.Add(h.HITId);
        }

        [Test, Description("Creates a hit by reading the HTMLQuestion from a question file")]
        public void CreateHitFromHTMLQuestionFile()
        {
            HIT h = TestUtil.CreateSingleHit("CreateHit", "Question created from HIT", Path.Combine(TestUtil.DataPath, "HTMLQuestion.xml"), rand.Next(5));
            TestUtil.CreatedHits.Add(h.HITId);
        }

        [Test, Description("Creates a hit without wrapping the question in XML first. Tests wether the implicit wrapping mechanism works")]
        public void CreateHitWithoutXmlQuestion()
        {
            bool b = TestUtil.Client.Config.EnsureQuestionsAreXmlWrappedBeforeSending;
            TestUtil.Client.Config.EnsureQuestionsAreXmlWrappedBeforeSending = true;

            CreateHITRequest req = TestUtil.GetCreateHitRequest("CreateHitWithoutXmlQuestion", "What are our options?", string.Format("Do you have more than {0} grand?", rand.Next(100, 50000)), rand.Next(5), null);
            req.Question = "What are our options outside of XML?";
            HIT h = TestUtil.Client.CreateHIT(req);

            TestUtil.CreatedHits.Add(h.HITId);
            TestUtil.Client.Config.EnsureQuestionsAreXmlWrappedBeforeSending = b;
        }

        [Test, Description("Creates a hit without wrapping the question in XML first. Should result in a validation failure after sending")]
        [ExpectedException(typeof(ParseErrorException))]
        public void CreateHitWithoutXmlQuestionWithImplicitWrappingDisabled()
        {
            bool b1 = TestUtil.Client.Config.EnsureQuestionsAreXmlWrappedBeforeSending;
            bool b2= TestUtil.Client.Config.ValidateQuestionBeforeSending;
            TestUtil.Client.Config.EnsureQuestionsAreXmlWrappedBeforeSending = false;
            TestUtil.Client.Config.ValidateQuestionBeforeSending = false;

            try
            {
                CreateHITRequest req = TestUtil.GetCreateHitRequest("CreateHitWithoutXmlQuestion", "What are our options?", string.Format("Do you have more than {0} grand?", rand.Next(100, 50000)), rand.Next(5), null);
                req.Question = "What are our options outside of XML?";
                TestUtil.Client.CreateHIT(req);
            }
            finally
            {
                TestUtil.Client.Config.EnsureQuestionsAreXmlWrappedBeforeSending = b1;
                TestUtil.Client.Config.ValidateQuestionBeforeSending = b2;
            }
            
        }


        [Test, Description("Creates a hit with implicit QAP validation. Should result in a validation failure before sending the request")]
        [ExpectedException(typeof(System.Xml.Schema.XmlSchemaValidationException))]        
        public void CreateHitWithInvalidXmlFileAndImplicitValidation()
        {
            bool b = TestUtil.Client.Config.ValidateQuestionBeforeSending;
            TestUtil.Client.Config.ValidateQuestionBeforeSending = true;

            CreateHITRequest req = TestUtil.GetCreateHitRequest("CreateHitWithoutXmlQuestion", "What are our options?", TestUtil.DataPath + "/QuestionForm.xml", rand.Next(5), null);
            req.Question = req.Question.Replace("<IsRequired>true</IsRequired>", "<IsRequired>_NOTABOOL_</IsRequired>");

            try
            {
                HIT h = TestUtil.Client.CreateHIT(req);
            }
            finally
            {
                TestUtil.Client.Config.ValidateQuestionBeforeSending = b;
            }
        }

        [Test, Description("Creates a hit with a qualification ID")]
        public void CreateHitWithQualification()
        {
            HIT h = TestUtil.CreateSingleHit("CreateHitWithQualification", 
                "This is a question created by the mturk .Net SDK", 
                string.Format("What's the next prime number that is > {0}?", rand.Next(100, 50000)), 
                rand.Next(5),
                TestUtil.GetExistingQualificationTypeID());

            TestUtil.CreatedHits.Add(h.HITId);
        }
		
		[Test, Description("Loads an existing HIT")]
		public void GetHit()
		{
			HIT h1 = TestUtil.CreateSingleHit("GetHit", "This is a question created by the mturk .Net SDK", string.Format("Can you draw a pretty picture of the next prime number < {0}?", rand.Next(100, 5000)), rand.Next(2));
			HIT h2 = TestUtil.LoadHitByID(h1.HITId);

            TestUtil.CreatedHits.Add(h1.HITId);
			
			Assert.IsNotNull(h2.Question, "No question found in HIT");
			Assert.IsNotNull(h2.Description, "No description found in HIT");				
			Assert.IsNotNull(h2.Title, "No title found in HIT");
		}
		
		[Test, Description("Disables an existing HIT")]
		public void DisableHit()
		{
			HIT h1 = TestUtil.CreateSingleHit("DisableHit", "This is a question created by the mturk .Net SDK", "test", 10);

            TestUtil.CreatedHits.Add(h1.HITId);
			
			DisableHITRequest request = new DisableHITRequest();
			request.HITId = h1.HITId;			
			
			TestUtil.Client.DisableHIT(request);
		}
		
		[Test, Description("Extends an existing HIT")]
		public void ExtendHit()
		{
			HIT h1 = TestUtil.CreateSingleHit("ExtendHit", "This is a question created by the mturk .Net SDK", "test", 10);
			HIT h2 = TestUtil.LoadHitByID(h1.HITId);

            TestUtil.CreatedHits.Add(h1.HITId);
			
			ExtendHITRequest request = new ExtendHITRequest();
			request.HITId = h1.HITId;		
			request.ExpirationIncrementInSeconds = 3600;
			request.ExpirationIncrementInSecondsSpecified = true;
			request.MaxAssignmentsIncrement = 1;
			request.MaxAssignmentsIncrementSpecified = true;

            TestUtil.Client.ExtendHIT(request);			
			
			// check changes
			HIT h3 = TestUtil.LoadHitByID(h1.HITId);
			TimeSpan deltaExpiry = h3.Expiration - h2.Expiration;
			int deltaAssignments = h3.MaxAssignments - h2.MaxAssignments;
			
			Assert.IsTrue(deltaExpiry.TotalSeconds == 3600, "Expiration date not properly updated");
			Assert.IsTrue(deltaAssignments == 1, "MaxAssignments not properly updated");
		}
		
		[Test, Description("Expires an existing HIT")]
		public void ExpireHit()
		{
			HIT h1 = TestUtil.CreateSingleHit("ExpireHit", "This is a question created by the mturk .Net SDK", "test", 10);
			HIT h2 = TestUtil.LoadHitByID(h1.HITId);

            TestUtil.CreatedHits.Add(h1.HITId);
			TestUtil.ExpireHitByID(h1.HITId);
						
			// load and check expiration date			
			HIT h3 = TestUtil.LoadHitByID(h1.HITId);
			Assert.IsTrue(h2.HITStatus.Equals(HITStatus.Assignable) && h3.HITStatus.Equals(HITStatus.Reviewable), "Hit not expired");
		}	
		
		[Test, Description("Disposes an existing HIT")]
		public void DisposeHit()
		{
			HIT h1 = TestUtil.CreateSingleHit("ExpireHit", "This is a question created by the mturk .Net SDK", "test", 10);
			// expire it so it becomes reviewable
            TestUtil.ExpireHitByID(h1.HITId);
			
			DisposeHITRequest request = new DisposeHITRequest();
			request.HITId = h1.HITId;
			
			TestUtil.Client.DisposeHIT(request);
						
			// load and check expiration date			
			HIT h2 = TestUtil.LoadHitByID(h1.HITId);
			Assert.IsTrue(h2.HITStatus.Equals(HITStatus.Disposed), "Hit not disposed");
		}

        [Test, Description("Gets existing HITs")]
		public void SearchHit()
		{
			SearchHITsRequest request = new SearchHITsRequest();			
			SearchHITsResult result = TestUtil.Client.SearchHITs(request);
			
			Assert.IsTrue(result.NumResults > 0, "No HITs found");
		}

        [Test, Description("Gets reviewable HITs")]
		public void GetReviewableHITs()
		{
			GetReviewableHITsRequest request = new GetReviewableHITsRequest();

            GetReviewableHITsResponse response = (GetReviewableHITsResponse)TestUtil.Client.SendRequest(request);
			
			Assert.IsTrue(response.GetReviewableHITsResult[0].NumResults > 0, "No reviewable HITs found");
		}

        [Test, Description("Sets the HIT status of an existing HIT as reviewing")]
		public void SetHITAsReviewing()
		{
            string hitID = TestUtil.GetReviewableHIT();

            if (hitID != null)
            {                 
                SetHITAsReviewingRequest request = new SetHITAsReviewingRequest();
                request.HITId = hitID;
                request.Revert = false;

                SetHITAsReviewingResponse response = (SetHITAsReviewingResponse)TestUtil.Client.SendRequest(request);

                HIT hit1 = TestUtil.LoadHitByID(hitID);
                Assert.IsTrue(hit1.HITStatus.Equals(request.Revert ? HITStatus.Reviewable : HITStatus.Reviewing));
            }            
		}

        [Test, Description("Gets the assignments for a reviewable HIT")]
        public void GetAssignmentsForHIT()
		{
            string hitID = TestUtil.GetReviewableHIT();

            if (hitID != null)
            {
                Assignment[] assignments = TestUtil.GetAssignmentsForHIT(hitID);
            }
		}

        [Test, Description("Sends an email reminder to the workers of a reviable HIT")]
        public void NotifyWorkers()
        {
            string hitID = TestUtil.GetReviewableHIT();

            if (hitID != null)
            {
                Assignment[] assignments = TestUtil.GetAssignmentsForHIT(hitID);

                if (assignments != null)
                {
                    NotifyWorkersRequest request = new NotifyWorkersRequest();
                    request.WorkerId = new string[] { assignments[0].WorkerId };
                    request.Subject = "Reminder to work on task " + assignments[0].HITId;
                    request.MessageText = "Please login@mturk and work on this";

                    NotifyWorkersResponse response = (NotifyWorkersResponse)TestUtil.Client.SendRequest(request);
                }
            }
        }

        [Test, Description("Creates a HIT type")]
        public void CreateHitType()
        {
            string id = TestUtil.CreateSingleHitType();
            Assert.IsNotNull(id, "Failed to create hit type");
        }

        [Test, Description("Creates HITs for a specific HIT type")]
        public void CreateHitsForType()
        {
            string id = TestUtil.CreateSingleHitType();
            Assert.IsNotNull(id, "Failed to create hit type");

            int num = 3;
            string[] hitIDs = TestUtil.CreateHitsForType(id, num);

            Assert.IsTrue(hitIDs.Length == num, "Failed to create curResult for type");
        }

        [Test, Description("Sets notification options for a HIT type")]
        public void SetHITTypeNotification()
        {
            string email = TestUtil.Client.Config.GetConfig("HitTest.SetHITTypeNotification.Email", null);
            if (email==null)
            {
                System.Diagnostics.Trace.WriteLine("Ignoring test 'SetHITTypeNotification': No email setup for notifications. Please configure 'HitTest.SetHITTypeNotification.Email' to execute this test.");
            }
            else
            {
                string id = TestUtil.CreateSingleHitType();
                Assert.IsNotNull(id, "Failed to create hit type");

                SetHITTypeNotificationRequest req = new SetHITTypeNotificationRequest();
                req.Notification = new NotificationSpecification();
                req.Notification.Destination = TestUtil.Client.Config.GetConfig("HitTest.SetHITTypeNotification.Email", null);
                req.Notification.Transport = NotificationTransport.Email;
                req.Notification.Version = TestUtil.Client.Config.GetConfig("HitTest.SetHITTypeNotification.Version", "2006-05-05"); 
                req.Notification.EventType = new EventType[] { EventType.HITReviewable, EventType.HITExpired };
                req.Active = true;
                req.HITTypeId = id;

                SetHITTypeNotificationResponse response = (SetHITTypeNotificationResponse)TestUtil.Client.SendRequest(req);

                // send a test notification
                SendTestEventNotificationRequest req1 = new SendTestEventNotificationRequest();
                req1.Notification = req.Notification;
                req1.TestEventType = EventType.HITReviewable;
                req1.TestEventTypeSpecified = true;

                SendTestEventNotificationResponse response1 = (SendTestEventNotificationResponse)TestUtil.Client.SendRequest(req1);
            }
        }

        [Test, Description("Creates a HIT that uses an idempotency token")]
        public void CreateHITWithIdempotency()
        {
            string hitTypeId = TestUtil.CreateSingleHitType();
            Assert.IsNotNull(hitTypeId, "Failed to create hit type");
            TestUtil.CreatedHitTypes.Add(hitTypeId);

            string token = Guid.NewGuid().ToString();

            CreateHITRequest req = new CreateHITRequest();
            req.HITTypeId = hitTypeId;
            req.Description = "Idempotency test hit created by Mechanical Turk .Net SDK " + token;
            req.MaxAssignments = 1;
            req.Question = QuestionUtil.ConvertSingleFreeTextQuestionToXML("Hello");
            req.Reward = new Price();
            req.Reward.Amount = 1;
            req.Reward.CurrencyCode = "USD";
            req.Title = "Hello <something>";
            req.AssignmentDurationInSeconds = 60;
            req.LifetimeInSeconds = 3600;
            req.UniqueRequestToken = token;

            HIT hit = TestUtil.Client.CreateHIT(req);
            Assert.IsNotNullOrEmpty(hit.HITId);
            TestUtil.CreatedHits.Add(hit.HITId);

            // Second create should throw an exception with the existing HIT Id
            try
            {
                HIT hit2 = TestUtil.Client.CreateHIT(req);
            }
            catch (ServiceException e) {
                Assert.IsTrue(e.Message.Contains(hit.HITId));
            }

            // Third create with a different token should return a different HIT ID
            req.UniqueRequestToken = Guid.NewGuid().ToString();
            HIT hit3 = TestUtil.Client.CreateHIT(req);
            TestUtil.CreatedHits.Add(hit3.HITId);
            Assert.AreNotEqual(hit3.HITId, hit.HITId);
        }

        [Test, Description("Creates a HIT that uses the Score My Known Answers assignment review policy")]
        public void CreateHITWithAssignmentReviewPolicy()
        {
            string id = TestUtil.CreateSingleHitType();
            Assert.IsNotNull(id, "Failed to create hit type");
            TestUtil.CreatedHitTypes.Add(id);

            ReviewPolicy policy = new ReviewPolicy();
            PolicyParameter[] parameters = new PolicyParameter[1];
            PolicyParameter answers = new PolicyParameter();
            answers.Key = "AnswerKey";
            ParameterMapEntry[] mapEntries = new ParameterMapEntry[1];
            ParameterMapEntry answer1 = new ParameterMapEntry();
            answer1.Key = "1";
            answer1.Value = new string[1] { "World" };
            mapEntries[0] = answer1;
            answers.MapEntry = mapEntries;
            parameters[0] = answers;
            policy.PolicyName = "ScoreMyKnownAnswers/2011-09-01";
            policy.Parameter = parameters;

            CreateHITRequest req = new CreateHITRequest();
            req.HITTypeId = id;
            req.Description = "ScoreMyKnownAnswers test hit created by Mechanical Turk .Net SDK";
            req.AssignmentReviewPolicy = policy;
            req.MaxAssignments = 1;
            req.Question = QuestionUtil.ConvertSingleFreeTextQuestionToXML("Hello");
            req.Reward = new Price();
            req.Reward.Amount = 1;
            req.Reward.CurrencyCode = "USD";
            req.Title = "Hello <something>";
            req.AssignmentDurationInSeconds = 60;
            req.LifetimeInSeconds = 3600;

            HIT hit = TestUtil.Client.CreateHIT(req);
            Assert.IsNotNullOrEmpty(hit.HITId);

            TestUtil.CreatedHits.Add(hit.HITId);
        }

        [Test, Description("Creates a HIT that uses the Plurality hit review policy")]
        public void CreateHITWithHITReviewPolicy()
        {
            string id = TestUtil.CreateSingleHitType();
            Assert.IsNotNull(id, "Failed to create hit type");
            TestUtil.CreatedHitTypes.Add(id);

            ReviewPolicy policy = new ReviewPolicy();
            PolicyParameter[] parameters = new PolicyParameter[2];
            PolicyParameter questionIds = new PolicyParameter();
            questionIds.Key = "QuestionIds";
            questionIds.Value = new string[1] { "1" };
            parameters[0] = questionIds;
            PolicyParameter agreementThreshold = new PolicyParameter();
            agreementThreshold.Key = "QuestionAgreementThreshold";
            agreementThreshold.Value = new string[1] { "50" };
            parameters[1] = agreementThreshold;
            policy.PolicyName = "SimplePlurality/2011-09-01";
            policy.Parameter = parameters;

            CreateHITRequest req = new CreateHITRequest();
            req.HITTypeId = id;
            req.Description = "Plurality test hit created by Mechanical Turk .Net SDK";
            req.HITReviewPolicy = policy;
            req.MaxAssignments = 3;
            req.Question = QuestionUtil.ConvertSingleFreeTextQuestionToXML("Hello");
            req.Reward = new Price();
            req.Reward.Amount = 1;
            req.Reward.CurrencyCode = "USD";
            req.Title = "Hello <something>";
            req.AssignmentDurationInSeconds = 60;
            req.LifetimeInSeconds = 3600;

            HIT hit = TestUtil.Client.CreateHIT(req);
            Assert.IsNotNullOrEmpty(hit.HITId);

            TestUtil.CreatedHits.Add(hit.HITId);
        }

        [Test, Description("Creates a HIT with a hit review policy and obtains review results")]
        public void GetReviewResults()
        {
            string id = TestUtil.CreateSingleHitType();
            Assert.IsNotNull(id, "Failed to create hit type");
            TestUtil.CreatedHitTypes.Add(id);

            ReviewPolicy policy = new ReviewPolicy();
            PolicyParameter[] parameters = new PolicyParameter[2];
            PolicyParameter questionIds = new PolicyParameter();
            questionIds.Key = "QuestionIds";
            questionIds.Value = new string[1] { "1" };
            parameters[0] = questionIds;
            PolicyParameter agreementThreshold = new PolicyParameter();
            agreementThreshold.Key = "QuestionAgreementThreshold";
            agreementThreshold.Value = new string[1] { "50" };
            parameters[1] = agreementThreshold;
            policy.PolicyName = "SimplePlurality/2011-09-01";
            policy.Parameter = parameters;

            CreateHITRequest req = new CreateHITRequest();
            req.HITTypeId = id;
            req.Description = "Plurality test hit created by Mechanical Turk .Net SDK";
            req.HITReviewPolicy = policy;
            req.MaxAssignments = 1;
            req.Question = QuestionUtil.ConvertSingleFreeTextQuestionToXML("Hello");
            req.Reward = new Price();
            req.Reward.Amount = 1;
            req.Reward.CurrencyCode = "USD";
            req.Title = "Hello <something>";
            req.AssignmentDurationInSeconds = 60;
            req.LifetimeInSeconds = 3600;

            HIT hit = TestUtil.Client.CreateHIT(req);
            Assert.IsNotNullOrEmpty(hit.HITId);
            TestUtil.CreatedHits.Add(hit.HITId);

            GetReviewResultsForHITRequest reviewRequest = new GetReviewResultsForHITRequest();
            reviewRequest.HITId = hit.HITId;
            GetReviewResultsForHITResult result = TestUtil.Client.GetReviewResultsForHIT(reviewRequest);
            Assert.AreEqual("SimplePlurality/2011-09-01", result.HITReviewPolicy.PolicyName);
        }

        [Test, Description("Extends a HIT using an idempotency token")]
        public void ExtendHITWithIdempotency()
        {
            string token = Guid.NewGuid().ToString();
            HIT hit = TestUtil.CreateSingleHit("Test extend", "Test HIT created by the .Net SDK", "Answer the question", 1);

            DateTime oldExpiry = TestUtil.LoadHitByID(hit.HITId).Expiration;

            ExtendHITRequest req = new ExtendHITRequest();
            req.ExpirationIncrementInSeconds = 3600;
            req.HITId = hit.HITId;
            req.UniqueRequestToken = token;

            
            TestUtil.Client.ExtendHIT(req);

            // Check that the expiration time has increased
            DateTime newExpiry = TestUtil.LoadHitByID(hit.HITId).Expiration;
            Assert.IsTrue(newExpiry.CompareTo(oldExpiry) > 0);

            // Extend again
            try
            {
                TestUtil.Client.ExtendHIT(req);
            }
            catch (ServiceException e)
            {
                Assert.IsTrue(e.Message.Contains("has already been processed"));
                Assert.IsTrue(e.Message.Contains(token));
            }
            // Check that the expiration time has not increased
            DateTime unchangedExpiry = TestUtil.LoadHitByID(hit.HITId).Expiration;
            Assert.AreEqual(newExpiry, unchangedExpiry);
        }
    }
}
