#region Copyright & license notice
/*
 * Copyright: Copyright (c) 2007 Amazon Technologies, Inc.
 * License:   Apache License, Version 2.0
 */
#endregion

using System;
using System.IO;
using NUnit.Framework;
using Amazon.WebServices.MechanicalTurk.Domain;
using Amazon.WebServices.MechanicalTurk.Exceptions;


namespace Amazon.WebServices.MechanicalTurk.Test
{
	[TestFixture]
	public class QualificationTest
	{
        [TestFixtureSetUp]
        public void CheckSandbox()
        {
            TestUtil.CheckSandbox();
        }

        [Test, Description("Creates a simple qualification type")]
        public void CreateSimpleQualificationType()
		{
            TestUtil.CreateSimpleQualificationType(string.Format("No test - Autogrant ({0:g})", DateTime.Now), "Simple qualification requiring no test - autogranted");
        }

        [Test, Description("Creates a qualification type with a test question")]
        public void CreateQualificationTypeWithTest()
        {
            CreateQualificationTypeRequest req = new CreateQualificationTypeRequest();
            req.AutoGranted = false;
            req.QualificationTypeStatus = QualificationTypeStatus.Active;
            req.QualificationTypeStatusSpecified = true;
            req.Keywords = "MTurkQual, MTurkQualWithTest";
            req.Description = "Qualification requiring test";
            req.Name = string.Format("Qualification requiring test ({0:g})", DateTime.Now);
            //req.Test = TurkUtil.SerializeQuestionForm(TurkUtil.ReadQuestionFormFromFile(Path.Combine(TestUtil.DataPath, "QuestionForm.xml")));
            req.Test = QuestionUtil.ConvertMultipleFreeTextQuestionToXML(
                new string[] { "What's the difference between apples and oranges?", "What's 2+plus?" });          
            req.TestDurationInSeconds = 60;

            CreateQualificationTypeResponse response = (CreateQualificationTypeResponse)TestUtil.Client.SendRequest(req);
            Assert.IsNotNull(response.QualificationType[0].QualificationTypeId, "Failed to create simple qualification type. ID is null");
        }

        [Test, Description("Gets all requests for qualifications")]
        public void GetQualificationRequests()
        {
            GetQualificationRequestsRequest req = new GetQualificationRequestsRequest();
            GetQualificationRequestsResponse response = (GetQualificationRequestsResponse)TestUtil.Client.SendRequest(req);
        }

        [Test, Description("Gets all qualification requests matching the query param 'MTurkQual'")]
        public void SearchQualificationTypes()
        {
            SearchQualificationTypesRequest req = new SearchQualificationTypesRequest();
            req.Query = "MTurkQual";
            SearchQualificationTypesResponse response = (SearchQualificationTypesResponse)TestUtil.Client.SendRequest(req);

            Assert.IsTrue(response.SearchQualificationTypesResult[0].NumResults > 0);
            Assert.IsTrue(response.SearchQualificationTypesResult[0].TotalNumResults > 0);
        }

        [Test, Description("Returns all of the Qualifications granted to Workers for a given Qualification type")]
        public void GetQualificationsForQualificationType()
        {
            GetQualificationsForQualificationTypeRequest req = new GetQualificationsForQualificationTypeRequest();
            req.QualificationTypeId = TestUtil.GetExistingQualificationTypeID();
            GetQualificationsForQualificationTypeResponse response = (GetQualificationsForQualificationTypeResponse)TestUtil.Client.SendRequest(req);

            Assert.IsTrue(response.GetQualificationsForQualificationTypeResult[0].NumResults >= 0);
        }

        [Test, Description("Gets an exisiting qualification type")]
        public void GetQualificationType()
        {
            GetQualificationTypeRequest req = new GetQualificationTypeRequest();
            req.QualificationTypeId = TestUtil.GetExistingQualificationTypeID();
            GetQualificationTypeResponse response = (GetQualificationTypeResponse)TestUtil.Client.SendRequest(req);

            Assert.IsNotNull(response.QualificationType[0].Description, "Qualification not loaded properly");
        }

        [Test, Description("Updates an existing qualification type")]
        public void UpdateQualificationType()
        {
            UpdateQualificationTypeRequest req = new UpdateQualificationTypeRequest();
            req.QualificationTypeId = TestUtil.GetExistingQualificationTypeID();
            req.Description = "Updated description @ "+DateTime.Now.ToString("g") + "_" + DateTime.Now.Ticks;
            UpdateQualificationTypeResponse response = (UpdateQualificationTypeResponse)TestUtil.Client.SendRequest(req);
        }
	}
}
