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
using Amazon.WebServices.MechanicalTurk;
using Amazon.WebServices.MechanicalTurk.Domain;

namespace SimpleSurvey
{
    /// <summary>
    /// The Simple Survey sample application will create a HIT asking a worker to indicate their 
    /// political party preferences.
    /// 
    /// The following concepts are covered:
    /// - Loading of a question from a file
    /// - Using a locale qualification
    /// </summary>
    /// <remarks>
    /// NOTE: You will need to configure your AWS access key information in the application config (app.config)
    /// prior to running this sample
    /// </remarks>
    public class MTurkSimpleSurvey
    {
        private SimpleClient client = new SimpleClient();
        /// <summary>
        /// Check if there are enough funds in your account in order to create the HIT
        /// on Mechanical Turk
        /// </summary>
        /// <returns>true if there are sufficient funds. False if not.</returns>
        public bool HasEnoughFunds()
        {
            return (client.GetAvailableAccountBalance() > 0);
        }

        /// <summary>
        /// Creates the simple survey.
        /// </summary>
        public void CreateSimpleSurvey()
        {
            // This is an example of creating a qualification for a worker that must be based in the US
            QualificationRequirement qualReq = new QualificationRequirement();
            
            qualReq.QualificationTypeId = MTurkSystemQualificationTypes.LocaleQualification; // locale system qual identifier                
            
            Locale country = new Locale();
            country.Country = "US";
            qualReq.LocaleValue = country;
            qualReq.Comparator = Comparator.EqualTo;

            // The create HIT method takes in an array of QualificationRequirements
            // since a HIT can require multiple qualifications.
            List<QualificationRequirement> qualReqs = new List<QualificationRequirement>();
            qualReqs.Add(qualReq);

            // Loading the question (QAP) file 
            string questionFile = Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, "SimpleSurveyQuestion.xml");
            string question = File.ReadAllText(questionFile);

            //Creating the HIT and loading it into Mechanical Turk
            HIT h = client.CreateHIT(null, 
                "What is your political preference?",
                "This is a simple survey HIT created by MTurk SDK.", 
                "sample, .Net SDK, survey",
                question,
                1, 
                60 * 60,                // 1 hour
                60 * 60 * 24 * 15,      // 15 days
                60 * 60 * 24 * 3,       // 3 days,
                1, 
                "sample#survey",
                qualReqs,
                null 
              );

            // output ID and Url of new HIT (URL where HIT is available on the Mechanical Turk worker website)
            Console.WriteLine("Created HIT: {0} ({1})", h.HITId, client.GetPreviewURL(h.HITTypeId));
        }
    }
}
