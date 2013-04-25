#region Copyright & license notice
/*
 * Copyright: Copyright (c) 2007 Amazon Technologies, Inc.
 * License:   Apache License, Version 2.0
 */
#endregion

using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Amazon.WebServices.MechanicalTurk.Domain;


namespace Amazon.WebServices.MechanicalTurk.Test
{
    internal class TestUtil
    {
        private const string ALLOW_NON_SANDBOX_KEY = "MechanicalTurk.Test.UseNonSandboxEndpoint";

        private static MTurkConfig _config = new MTurkConfig();

        private static MTurkClient _client = new MTurkClient(_config);
        public static MTurkClient Client
        {
            get {return (_client);}
            set { _client = value; }
        }

        private static string _dataPath = null;
        /// <summary>
        /// Returns the folder where the test data is located
        /// </summary>
        public static string DataPath
        {
            get
            {
                if (_dataPath == null)
                {
                    string tmpDir = Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().CodeBase), "Data").Replace("file:\\", string.Empty); 
                    if (!Directory.Exists(tmpDir))
                    {
                        tmpDir = Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, "Data");
                        if (!Directory.Exists(tmpDir))
                        {
                            throw new Exception("Cannot locate data folder for SDK unit tests");
                        }                        
                    }

                    _dataPath = tmpDir;
                }

                return _dataPath;
            }
        }

        public static HIT SampleHITInstance
        {
            get
            {
                HIT h = new HIT();
                h.Description = "The task is to select one image out of three that best represents the provided topic given a set of evaluation criteria.";
                h.Title = "Select the best image for a given topic";
                h.Keywords = "sample, .Net SDK, best, image";
                h.Question = "What now?";
                h.MaxAssignments = 1;
                h.MaxAssignmentsSpecified = true;
                h.RequesterAnnotation = "sample#image";
                h.AssignmentDurationInSeconds = 3600;
                h.AssignmentDurationInSecondsSpecified = true;
                h.AutoApprovalDelayInSeconds = 1296000;
                h.AutoApprovalDelayInSecondsSpecified = true;
                //h.LifetimeInSeconds = 259200;

                h.Reward = new Price();
                h.Reward.Amount = new decimal(0.05);
                h.Reward.CurrencyCode = "USD";

                QualificationRequirement q1 = new QualificationRequirement();
                q1.QualificationTypeId = "00000000000000000071"; // locale system qual identifier 

                Locale country = new Locale();
                country.Country = "US";
                q1.LocaleValue = country;

                q1.Comparator = Comparator.EqualTo;
                q1.IntegerValue = 5;
                q1.IntegerValueSpecified = true;
                q1.RequiredToPreview = true;
                q1.RequiredToPreviewSpecified = true;

                QualificationRequirement q2 = new QualificationRequirement();
                q2.QualificationTypeId = MTurkSystemQualificationTypes.ApprovalRateQualification;

                q2.Comparator = Comparator.GreaterThan;
                q2.IntegerValue = 50;
                q2.IntegerValueSpecified = true;

                h.QualificationRequirement = new QualificationRequirement[] { q1, q2 };

                return h;
            }
        }

        #region Safety
        public static void CheckSandbox()
        {
            string endpoint = Client.Config.ServiceEndpoint;
            if (endpoint.Contains("sandbox")) {
                return;
            }
            if ("Use real money".Equals(Client.Config.GetConfig(ALLOW_NON_SANDBOX_KEY, null))) {
                return;
            }
            throw new InvalidOperationException("Endpoint " + endpoint + " is not a sandbox URL. Not running tests against this URL.");
        }
        #endregion

        #region Helpers
        public static HIT CreateSingleHit(string title, string description, string question, decimal price)
        {
            return CreateSingleHit(title, description, question, price, null);
        }

        // question param may be a filename or a string, which will be wrapped in a QuestionForm
        public static CreateHITRequest GetCreateHitRequest(string title, string description, string question, decimal price, string qualID)
        {
            CreateHITRequest req = new CreateHITRequest();
            req.Title = string.Format("{0} (Test suite: {1})", title, DateTime.Now.ToString("g"));
            req.Description = description;

            if (File.Exists(question))
            {
                req.Question = QuestionUtil.SerializeQuestionFileToString(question);
            }
            else
            {
                req.Question = QuestionUtil.ConvertSingleFreeTextQuestionToXML(question);
            }            

            Price p = new Price();
            p.Amount = price + 1;
            p.CurrencyCode = "USD";
            req.Reward = p;

            req.Keywords = null;
            req.AssignmentDurationInSeconds = 3600;
            req.AutoApprovalDelayInSeconds = 24 * 15 * 3600;
            req.LifetimeInSeconds = 24 * 3 * 3600;
            req.MaxAssignments = 1;

            if (qualID != null)
            {
                QualificationRequirement qual = new QualificationRequirement();
                qual.QualificationTypeId = qualID;
                qual.IntegerValue = 1;
                qual.IntegerValueSpecified = true;  
                req.QualificationRequirement = new QualificationRequirement[] { qual };
            }


            return req;
        }

        public static HIT CreateSingleHit(string title, string description, string question, decimal price, string qualID)
        {
            CreateHITRequest req = GetCreateHitRequest(title, description, question, price, qualID);

            HIT hit = TestUtil.Client.CreateHIT(req);
            Assert.IsNotNull(hit.HITId, "No HIT ID received");


            return hit;
        }

        public static HIT LoadHitByID(string id)
        {
            GetHITRequest request = new GetHITRequest();
            request.HITId = id;

            return TestUtil.Client.GetHIT(request);
        }

        public static void ExpireHitByID(string id)
        {
            ForceExpireHITRequest request = new ForceExpireHITRequest();
            request.HITId = id;

            TestUtil.Client.ForceExpireHIT(request);
        }

        public static Assignment[] GetAssignmentsForHIT(string hitID)
        {
            GetAssignmentsForHITRequest request = new GetAssignmentsForHITRequest();
            request.HITId = hitID;
            request.PageNumber = 1;
            request.PageSize = 10;

            GetAssignmentsForHITResponse response = (GetAssignmentsForHITResponse)TestUtil.Client.SendRequest(request);

            Assert.IsTrue(response.GetAssignmentsForHITResult.Length > 0, "No assignments found for request");

            return response.GetAssignmentsForHITResult[0].Assignment;
        }

        public static string CreateSingleHitType()
        {
            RegisterHITTypeRequest req = new RegisterHITTypeRequest();
            req.AssignmentDurationInSeconds = 60;
            req.Description = "Write a Haiku containing the words contained in this question";
            req.Keywords = "Haiku, poetry, madness";
            req.Reward = new Price();
            req.Reward.Amount = 1;
            req.Reward.CurrencyCode = "USD";
            req.Title = "Poetry challenge";

            RegisterHITTypeResponse response = (RegisterHITTypeResponse)TestUtil.Client.SendRequest(req);

            TestUtil.CreatedHitTypes.Add(response.RegisterHITTypeResult[0].HITTypeId);

            return response.RegisterHITTypeResult[0].HITTypeId;
        }

        public static string[] CreateHitsForType(string typeID, int num)
        {
            string[] keywords = new string[] { "Work", "Whale", "Indifference", "Hahahahaha", "Jellybeans", 
                "Sandwhich", "Outrage", "Innards", "Batman", "Fahrvergnuegen", "Reality", "Mine" };

            Random rnd = new Random();
            CreateHITRequest[] reqs = new CreateHITRequest[num];
            for (int i = 0; i < num; i++)
            {
                reqs[i] = new CreateHITRequest();
                reqs[i].HITTypeId = typeID;
                reqs[i].LifetimeInSeconds = 360 * 24;
                reqs[i].MaxAssignments = 1;

                string words = string.Empty;
                for (int j = 0; j < 3; j++)
                {
                    words += keywords[rnd.Next(keywords.Length - 1)] + " ";
                }
                reqs[i].Question = QuestionUtil.ConvertSingleFreeTextQuestionToXML(words);
            }

            CreateHITResponse response = (CreateHITResponse)TestUtil.Client.SendRequest(reqs);

            string[] ret = new string[response.HIT.Length];
            for (int i = 0; i < ret.Length; i++)
            {
                ret[i] = response.HIT[i].HITId;
                TestUtil.CreatedHitTypes.Add(ret[i]);
            }

            return ret;
        }

        public static string GetReviewableHIT()
        {
            string ret = null;

            GetReviewableHITsRequest requestHits = new GetReviewableHITsRequest();
            GetReviewableHITsResponse responseHits = (GetReviewableHITsResponse)TestUtil.Client.SendRequest(requestHits);

            if (responseHits.GetReviewableHITsResult[0].NumResults > 0)
            {
               ret = responseHits.GetReviewableHITsResult[0].HIT[0].HITId;
            }

            return ret;
        }

        public static void CreateSimpleQualificationType(string name, string description)
        {
            CreateQualificationTypeRequest req = new CreateQualificationTypeRequest();
            req.AutoGranted = true;
            req.QualificationTypeStatus = QualificationTypeStatus.Active;
            req.QualificationTypeStatusSpecified = true;
            req.Keywords = "MTurkQual, SimpleMTurkQual";
            req.Description = "Simple qualification requiring no test - autogranted";
            req.Name = string.Format("No test - Autogrant ({0:g}.{1})", DateTime.Now, DateTime.Now.Millisecond);

            CreateQualificationTypeResponse response = (CreateQualificationTypeResponse)TestUtil.Client.SendRequest(req);
            Assert.IsNotNull(response.QualificationType[0].QualificationTypeId, "Failed to create simple qualification type. ID is null");

            TestUtil.CreatedQualificationTypes.Add(response.QualificationType[0].QualificationTypeId);
        }

        public static string GetExistingHitID()
        {
            string ret = null;
            if (CreatedHits.Count == 0)
            {
                HIT h = CreateSingleHit("Test", "test", "test", 1);
                CreatedHits.Add(h.HITId);
            }

            ret = CreatedHits[0];
            CreatedHits.RemoveAt(0);

            return ret;
        }

        public static string GetExistingQualificationTypeID()
        {
            string ret = null;

            if (CreatedQualificationTypes.Count == 0)
            {
                CreateSimpleQualificationType("Test " + DateTime.Now.ToString("g") + "_" + DateTime.Now.Ticks.ToString(), "Test");
            }
            ret = CreatedQualificationTypes[0];
            CreatedQualificationTypes.RemoveAt(0);

            return ret;
        }


        #endregion

        #region Objects produced in the tests
        public static List<string> CreatedHits = new List<string>();
        public static List<string> CreatedHitTypes = new List<string>();
        public static List<string> CreatedQualificationTypes = new List<string>();
        #endregion
    }
}
