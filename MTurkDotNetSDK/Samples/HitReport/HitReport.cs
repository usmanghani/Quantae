#region Copyright & license notice
/*
 * Copyright: Copyright (c) 2007 Amazon Technologies, Inc.
 * License:   Apache License, Version 2.0
 */
#endregion

using System;
using System.Collections.Generic;
using System.Text;
using Amazon.WebServices.MechanicalTurk;
using Amazon.WebServices.MechanicalTurk.Domain;

namespace HitReport
{
    public class HitReport
    {
        /// <summary>
        /// The HitReport sample application will output information about all active HITs
        /// of the configured requester.
        /// 
        /// The following concepts are covered:
        /// - Enumerate all HITs using the GetAllHITsEnumerator method
        /// - Output HIT information
        /// </summary>
        /// <remarks>
        /// NOTE: You will need to configure your AWS access key information in the application config (app.config)
        /// prior to running this sample
        /// </remarks>
        public void PrintAllHits()
        {
            // enumerate through all HITs (rather than loading them 
            // all in memory using GetAllHITs())
            SimpleClient client = new SimpleClient();

            // print a header
            Console.WriteLine("HIT ID                Expiration date   Status          Review status   Title");
            Console.WriteLine("=============================================================================");

            foreach (HIT h in client.GetAllHITsIterator())
            {
                Console.WriteLine("{0,10}  {1:yyyy-mm-dd:hh:MM}  {2,-15} {3,-15} {4}",
                    h.HITId,
                    h.Expiration,
                    h.HITStatus,
                    h.HITReviewStatus,
                    h.Title);
            }
        }
    }
}
