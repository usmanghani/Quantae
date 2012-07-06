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


namespace Amazon.WebServices.MechanicalTurk.Test
{
	[TestFixture]
	public class XmlTest
	{


        [Test, Description("Tests that a valid question form complies with the QuestionForm schema")]
        public void ValidateQuestionFile()
        {
            XmlUtil.ValidateXML("QuestionForm.xsd", TestUtil.DataPath + "/QuestionForm.xml");
        }

        [Test, Description("Tests that a invalid question form fails to comply with the QuestionForm schema")]
        [ExpectedException(typeof(System.Xml.Schema.XmlSchemaValidationException))]
        public void ValidateInvalidQuestionFile()
        {
            XmlUtil.ValidateXML("QuestionForm.xsd", TestUtil.DataPath + "/QuestionForm.Invalid.xml");
        }

        [Test, Description("That an answers file can be deserialized")]
        public void LoadAnswersFile()
        {
            QuestionFormAnswers test = (QuestionFormAnswers)XmlUtil.DeserializeXML(typeof(QuestionFormAnswers), new StreamReader(TestUtil.DataPath + "/QuestionFormAnswers.xml"));

            Assert.IsTrue(test.Answer != null && test.Answer.Length > 0);
        }

        [Test, Description("Loads an external question from file")]
        public void LoadExternalQuestionFromFile()
        {
            string file = TestUtil.DataPath + "/ExternalQuestion.xml";

            ExternalQuestion q = QuestionUtil.ReadExternalQuestionFromFile(file);

            Assert.IsNotNull(q.ExternalURL);
            Assert.IsNotNull(q.FrameHeight);
        }

        [Test, Description("Tests escaping of arbitrary strings to XML encoding")]
        public void EscapeStrings()
        {
            TestString("Less than < - Greater than >.");
            TestString("<org>Me & you & everyone else we don't know</org>");
            TestString("<org>How do you spell \"consppicuyos\"?</org>");
        }

        public void TestString(string s)
        {
            string xml1 = XmlUtil.XmlEncode(s);
            string org = XmlUtil.XmlDecode(xml1);

            Assert.AreEqual(s, org, "Failed xml encoding for " + s);
        }

	}
}
