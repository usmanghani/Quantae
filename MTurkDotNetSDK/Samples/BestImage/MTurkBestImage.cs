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

namespace BestImage
{

    /// <summary>
    /// The Best Image sample application will create a HIT asking a worker to choose 
    /// the best image of three given a set of criteria.
    /// 
    /// The following concepts are covered:
    /// - Using the HIT object by loading a HIT from a properties file
    /// - File based question with formatted content and a basic system qualification
    /// - Validating the correctness of the question
    /// </summary>
    /// <remarks>
    /// NOTE: You will need to configure your AWS access key information in the application config (app.config)
    /// prior to running this sample
    /// </remarks>
    public class MTurkBestImage
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
        /// Creates the Best Image HIT
        /// </summary>
        public void CreateBestImage()
        {
            // load HIT from properties file
            string hitFile = Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, "BestImage.properties");

            // instantiate HIT model object from this file
            HIT hit = client.DeserializeHIT(hitFile, MTurkSerializationFormat.Property);

            // validate the question file for the HIT
            string questionFile = Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, "BestImage.Question.xml");
            hit.Question = File.ReadAllText(questionFile);
            XmlUtil.ValidateXML("QuestionForm.xsd", hit.Question);

            hit = client.CreateHIT(hit);

            // output ID and Url of new HIT (URL where HIT is available on the Mechanical Turk worker website)
            Console.WriteLine("Created HIT {0} (URL: {1})", hit.HITId, client.GetPreviewURL(hit.HITTypeId));
        }
    }
}
