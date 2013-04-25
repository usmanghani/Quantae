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
using System.Threading;
using NUnit.Framework;
using Amazon.WebServices.MechanicalTurk.Domain;
using Amazon.WebServices.MechanicalTurk.Exceptions;
using Amazon.WebServices.MechanicalTurk.Advanced;

namespace Amazon.WebServices.MechanicalTurk.Test
{
    /// <summary>
    /// Tests related to the simple client (convenience layer)
    /// More SOAP op tests are covered in HITTest and QualTest
    /// </summary>
    [TestFixture]
    public class SimpleClientTest
    {
        SimpleClient client = new SimpleClient();

        [TestFixtureSetUp]
        public void CheckSandbox()
        {
            TestUtil.CheckSandbox();
        }

        [Test, Description("Creates a HIT")]
        public void CreateHIT()
        {
            HIT h = client.CreateHIT("Test", "Created by SimpleClientTest.CreateHIT", 1, "Ideas?", 1);

            Assert.IsNotNull(h.HITId);
        }

        [Test, Description("Creates a QuestionForm HIT")]
        public void CreateHITWithQuestionForm()
        {
            QuestionForm qf = QuestionUtil.ReadQuestionFormFromFile(TestUtil.DataPath + "/QuestionForm.xml");
            HIT h = client.CreateHIT(null, "Test", "Created by SimpleclientTest.CreateHITWithQuestionForm", null, qf, 1, 60, 60, 60, 1, null, null, null);

            Assert.IsNotNull(h.HITId);
        }

        [Test, Description("Creates an ExternalQuestion HIT")]
        public void CreateHITWithExternalQuestion()
        {
            ExternalQuestion eq = QuestionUtil.ReadExternalQuestionFromFile(TestUtil.DataPath + "/ExternalQuestion.xml");
            HIT h = client.CreateHIT(null, "Test", "Created by SimpleclientTest.CreateHITWithExternalQuestionForm", null, eq, 1, 60, 60, 60, 1, null, null, null);
            Assert.IsNotNull(h.HITId);            
        }

        [Test, Description("Creates an HTMLQuestion HIT")]
        public void CreateHITWithHTMLQuestion()
        {
            HTMLQuestion hq = QuestionUtil.ReadHTMLQuestionFromFile(TestUtil.DataPath + "/HTMLQuestion.xml");
            HIT h = client.CreateHIT(null, "Test", "Created by SimpleclientTest.CreateHITWithHTMLQuestionForm", null, hq, 1, 60, 60, 60, 1, null, null, null);
            Assert.IsNotNull(h.HITId);
        }

        [Test, Description("Creates a HIT with a HITLayoutId and params")]
        [ExpectedException(typeof(HitLayoutDoesNotExistException))]
        public void CreateHITWithHITLayout()
        {
            //this test will fail.  However, if you create a HitLayoutId in the requester UI, you can put it in here and the
            //test will pass---assuming your layout requires a single parameter called 'param1'.
            string hitLayoutId = "INVALIDHITLAYOUTID";

            Dictionary<string,string> hitLayoutParameters = new Dictionary<string,string>();
            hitLayoutParameters.Add("param1", "a test value");
            
            HIT h = client.CreateHIT(null, "Test", "Created by SimpleclientTest.CreateHITWithHITLayout",
                null, hitLayoutId, hitLayoutParameters, 1, 60, 60, 60, 1, null, null, null);

            Assert.IsNotNull(h.HITId);
        }

        [Test, Description("Tests serialization of a hit to and from XML format")]
        public void SerializeAndDeserializeHitToXML()
        {
            HIT h1 = TestUtil.SampleHITInstance;
            client.SerializeHIT(h1, "Hit.xml", MTurkSerializationFormat.Xml);

            HIT h2 = client.DeserializeHIT("Hit.xml", MTurkSerializationFormat.Xml);

            Assert.AreEqual(h1.Title, h2.Title);
            Assert.AreEqual(h1.Description, h2.Description);
            Assert.AreEqual(h1.QualificationRequirement.Length, h2.QualificationRequirement.Length);
            Assert.AreEqual(h1.QualificationRequirement[1].IntegerValue, h2.QualificationRequirement[1].IntegerValue);
            Assert.AreEqual(h1.QualificationRequirement[1].IntegerValueSpecified, h2.QualificationRequirement[1].IntegerValueSpecified);
        }


        [Test, Description("Tests serialization of a hit to and from property format")]
        public void SerializeAndDeserializeHitToProperties()
        {
            HIT h1 = TestUtil.SampleHITInstance;
            client.SerializeHIT(h1, "Hit.txt", MTurkSerializationFormat.Property);

            HIT h2 = client.DeserializeHIT("Hit.txt", MTurkSerializationFormat.Property);

            Assert.AreEqual(h1.Title, h2.Title);
            Assert.AreEqual(h1.Description, h2.Description);
            Assert.AreEqual(h1.QualificationRequirement[1].IntegerValue, h2.QualificationRequirement[1].IntegerValue);
            Assert.AreEqual(h1.QualificationRequirement[1].IntegerValueSpecified, h2.QualificationRequirement[1].IntegerValueSpecified);
            Assert.AreEqual(h1.QualificationRequirement[1].Comparator, h2.QualificationRequirement[1].Comparator);
        }

        [Test, Description("Deserializes HIT data from a file and then creates the HIT")]
        public void CreateHitFromPropertyFile()
        {
            HIT h = client.DeserializeHIT(TestUtil.DataPath+"/Hit.properties", MTurkSerializationFormat.Property);

            h = client.CreateHIT(h);

            Assert.IsNotNull(h.HITId);
            Assert.IsNotNull(h.HITTypeId);
        }

        [Test, Description("Creates a HIT with a qualification type")]
        public void CreateHitWithQual()
        {
            HIT h = client.DeserializeHIT(TestUtil.DataPath + "/Hit.properties", MTurkSerializationFormat.Property);

            QualificationType t = client.CreateQualificationType("Twenty twenty "+DateTime.Now.Ticks.ToString(), "eyesight", "Make sure you have the right glasses", QualificationTypeStatus.Active,
                360, "What's the difference between 0 and O?", null, 60, false, null);

            QualificationRequirement qual = new QualificationRequirement();
            qual.QualificationTypeId = t.QualificationTypeId;
            qual.IntegerValue = 1;
            qual.IntegerValueSpecified = true;
            h.QualificationRequirement = new QualificationRequirement[] { qual };

            h.Title = "Select the very best image";
            client.CreateHIT(h);
        }

        [Test, Description("Creates a HIT with a auto-approved qualification type")]
        public void CreateHitWithAutoGrantedQual()
        {
            HIT h = client.DeserializeHIT(TestUtil.DataPath + "/Hit.properties", MTurkSerializationFormat.Property);

            QualificationType t = client.CreateQualificationType("Canyoudoit " + DateTime.Now.Ticks.ToString(), null,
                "Confidence test", QualificationTypeStatus.Active,
                360, null, null, null, true, null);

            QualificationRequirement qual = new QualificationRequirement();
            qual.QualificationTypeId = t.QualificationTypeId;
            qual.IntegerValue = 1;
            qual.IntegerValueSpecified = true;
            qual.Comparator = Comparator.EqualTo;
            h.QualificationRequirement = new QualificationRequirement[] { qual };

            h.Title = "Select the absolute best image of all times ever 1";
            client.CreateHIT(h);
        }

        [Test, Description("Enumerates through all existing HITs")]
        public void EnumerateAllHits()
        {
            int count = 0;
            foreach (HIT h in client.GetAllHITsIterator())
            {
                count++;
                Assert.IsNotNull(h.HITId);
                HIT h2 = client.GetHIT(h.HITId);
                Assert.IsNotNull(h2.HITId);
                if (h2.NumberOfAssignmentsCompleted > 0)
                {
                    foreach (Assignment a in client.GetAllAssignmentsForHITIterator(h2.HITId))
                    {
                        Assert.IsNotNull(a.AssignmentId);
                        Assignment a2 = client.GetAssignment(a.AssignmentId, null).Assignment;
                        Assert.IsNotNull(a2.AssignmentId);
                    }
                }
            }

            count = 0;
        }

        [Test, Description("Enumerates through all existing qualification types")]
        public void EnumerateAllQualificationTypes()
        {
            foreach (QualificationType qual in client.GetAllQualificationTypesIterator())
            {
                Assert.IsNotNull(qual.QualificationTypeId);
            }
        }

        [Test, Description("Enumerates through all existing worker blocks")]
        public void EnumerateAllWorkerBlocks()
        {
            foreach (WorkerBlock block in client.GetAllBlockedWorkersIterator())
            {
                Assert.IsNotNull(block.WorkerId);
            }
        }
    }
}
