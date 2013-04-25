#region Copyright & license notice
/*
 * Copyright: Copyright (c) 2007 Amazon Technologies, Inc.
 * License:   Apache License, Version 2.0
 */
#endregion

using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using Amazon.WebServices.MechanicalTurk;
using Amazon.WebServices.MechanicalTurk.Domain;
using Amazon.WebServices.MechanicalTurk.Exceptions;

namespace Amazon.WebServices.MechanicalTurk.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                TestUtil.CheckSandbox();
                // simple entry point for simple debugging of tests within VS
                HitTest hitTest = new HitTest();
                XmlTest miscTest = new XmlTest();
                QualificationTest qualTest = new QualificationTest();
                MTurkClientTest clientTest = new MTurkClientTest();
                SimpleClientTest simpleClientTest = new SimpleClientTest();

//                clientTest.setup();
//                clientTest.SendSingleRequestWithoutEnvelope();

//                simpleClientTest.CreateHIT();
                //simpleClientTest.CreateHITWithQuestionForm();
                //simpleClientTest.CreateHITWithExternalQuestionForm();
                //simpleClientTest.SerializeAndDeserializeHitToXML();
                //simpleClientTest.SerializeAndDeserializeHitToProperties();
                //simpleClientTest.CreateHitFromPropertyFile();
                //simpleClientTest.CreateHitWithQual();
                //simpleClientTest.CreateHitWithAutoGrantedQual();
                //simpleClientTest.EnumerateAllHits();
                //simpleClientTest.EnumerateAllQualificationTypes();

                //hitTest.CreateHit();
                //hitTest.CreateHitWithoutXmlQuestion();
                //hitTest.CreateHitWithoutXmlQuestionWithImplicitWrappingDisabled();
                //hitTest.CreateHitWithInvalidXmlFileAndImplicitValidation();
                //hitTest.CreateHitFromQuestionFile();
                //hitTest.CreateHitWithQualification();
                //hitTest.DisposeHit();
                //hitTest.GetReviewableHITs();
                //hitTest.SetHITAsReviewing();
                //hitTest.GetAssignmentsForHIT();
                //hitTest.NotifyWorkers();
                //hitTest.SearchHit();
                //hitTest.CreateHitType();
                //hitTest.CreateHitsForType();
                //hitTest.SetHITTypeNotification();

//                hitTest.CreateHITWithAssignmentReviewPolicy();
//                hitTest.CreateHITWithHITReviewPolicy();
//                hitTest.CreateHITWithIdempotency();
//                hitTest.GetReviewResults();
                hitTest.ExtendHITWithIdempotency();

                //qualTest.CreateSimpleQualificationType();
                //qualTest.CreateQualificationTypeWithTest();
                //qualTest.GetQualificationType();
                //qualTest.UpdateQualificationType();
                //qualTest.SearchQualificationTypes();
                //qualTest.GetQualificationsForQualificationType();

                //clientTest.SendRequestToInvalidURL();
                //clientTest.SendRequestToInvalidServiceEndpoint();
                //clientTest.UseDifferentThrottler();
                //clientTest.SendRequestsMultithreaded();

                //miscTest.SerializeAndDeserializeObjectToXML();
                //miscTest.SerializeAndDeserializeObjectToProperties();
                //miscTest.ValidateQuestionFile();
                //miscTest.ValidateInvalidQuestionFile();
                //miscTest.LoadAnswersFile();
                //miscTest.EscapeStrings();
            }
            catch (ServiceException svcEx)
            {
                Debug.WriteLine(string.Format("Error '{0}': {1}", svcEx.ErrorCode, svcEx.Message));
                Debug.WriteLine(svcEx.Message);
            }          
        }
    }
}
