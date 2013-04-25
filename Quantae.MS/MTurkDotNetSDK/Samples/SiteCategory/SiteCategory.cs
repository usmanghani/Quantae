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
using System.Data;
using System.Data.OleDb;
using Amazon.WebServices.MechanicalTurk;
using Amazon.WebServices.MechanicalTurk.Domain;


namespace SiteCategory
{
    /// <summary>
    /// The Site Category sample application will create 5 HITs asking workers to categorize 
    /// websites into predefined categories.
    /// 
    /// The following concepts are covered:
    /// - Bulk load HITs using an CSV input file as datasource
    /// - Loading of HITs from a property file
    /// </summary>
    /// <remarks>
    /// NOTE: You will need to configure your AWS access key information in the application config (app.config)
    /// prior to running this sample
    /// </remarks>
    public class MTurkSiteCategory
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
        /// Creates the HITs by merging in the data from the CSV feed in the hit properties and question template
        /// </summary>
        public void CreateSiteCategoryHITs()
        {
            // read the site feed in a data table
            DataTable dtSites = ReadSites(Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, "SiteCategory.csv"));
            DataRow headerRow = dtSites.Rows[0];
            int numColumns = dtSites.Columns.Count;

            // create template vars
            string[] tplVars = new string[numColumns];
            for (int i=0; i<numColumns; i++)
            {
                tplVars[i] = "${"+dtSites.Columns[i].ColumnName+"}";
            }

            // read the HIT and question template
            string tplHIT = File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, "SiteCategory.properties"));
            string tplQuestion = File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, "SiteCategory.Question.xml"));

            // enumerate the rows and create HITs
            DataRow curRow;
            string curHIT = null;
            string curQuestion = null;
            for (int i=0; i<dtSites.Rows.Count; i++)
            {
                Console.Write("{0}. ", i+1);
                try
                {                    
                    curRow = dtSites.Rows[i];

                    for (int j = 0; j < numColumns; j++)
                    {
                        curHIT = tplHIT.Replace(tplVars[j], curRow[j] as string);
                        curQuestion = tplQuestion.Replace(tplVars[j], System.Web.HttpUtility.UrlEncode(curRow[j] as string));
                    }

                    HIT hit = client.DeserializeHIT(curHIT, MTurkSerializationFormat.Property);
                    hit.Question = curQuestion;

                    hit = client.CreateHIT(hit);

                    // output ID and Url of new HIT (URL where HIT is available on the Mechanical Turk worker website)
                    Console.WriteLine("Created HIT {0} (URL: {1})", hit.HITId, client.GetPreviewURL(hit.HITTypeId));
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Failed to create HIT: {0}", ex.Message);
                }
            }
        }

        #region Helpers
        private DataTable ReadSites(string csvFile)
        {
            DataTable ret = new DataTable();

            string connectionString = string.Format("Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Extended Properties=\"Text;HDR=YES\"", Path.GetDirectoryName(csvFile));
            string cmd = string.Format("Select * from {0}", Path.GetFileName(csvFile));
            using (OleDbConnection conn = new OleDbConnection(connectionString))
            {
                conn.Open();

                using (OleDbDataAdapter adapter = new OleDbDataAdapter(cmd, conn))
                {
                    adapter.Fill(ret);
                }                
            }

            return ret;
        }
        #endregion

    }
}
