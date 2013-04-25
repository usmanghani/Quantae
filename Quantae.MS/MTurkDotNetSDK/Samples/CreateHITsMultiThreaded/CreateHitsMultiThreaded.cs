#region Copyright & license notice
/*
 * Copyright: Copyright (c) 2008 Amazon Technologies, Inc.
 * License:   Apache License, Version 2.0
 */
#endregion

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Threading;
using Amazon.WebServices.MechanicalTurk;
using Amazon.WebServices.MechanicalTurk.Domain;
using Amazon.WebServices.MechanicalTurk.Exceptions;

namespace CreateHitsMultiThreaded	
{
	/// <summary>
	/// This application creates multiple HITs using multithreading. The application 
	/// requires two input files. The first is a text file that contains the questions 
	/// for the HITs. There is one question per line in the file.
    /// 
    /// The second file is a text file that contains the properties for the HITs. There is 
    /// one property per line with the format property:value.
    /// 
    /// Before you run this application, you need to configure your AWS access key information 
    /// in the application config (app.config).
    /// </summary>
    class CreateHitsMultiThreaded
    {
        #region Attributes
        private SimpleClient client = new SimpleClient();
        private String[] questions;
        private AutoResetEvent workCompleteEvent = new AutoResetEvent(false);
        private int numHits;
        private HIT hitTemplate;
        /// <summary>
        /// sets the number of threads for this process
        /// </summary>
        static int maxNumThreads = 5;
       
        /// <summary>
        /// count of successfully created HITs
        /// </summary>
        private int _successCount;
        public int SuccessCount
        {
            get { return (this._successCount); }
            set { this._successCount = value; }
        }

        /// <summary>
        /// count of HITs that were not created
        /// </summary>
        private int _failureCount;
        public int FailureCount
        {
            get { return (this._failureCount); }
            set { this._failureCount = value; }
        }

        private String Progress
        {        	
            get { return String.Format(" (Progress: {0}/{1})", SuccessCount + FailureCount, questions.Length); }
        }

        /// <summary>
        /// the file that contains the questions for the HITs
        /// </summary>
        private String _questionFile;       
        public String QuestionFile
        {
            get { return (this._questionFile); }
            set { this._questionFile = value; }
        }
        
        /// <summary>
        /// the file that contains the properties for the HITs
        /// </summary>
        private String _propertiesFile;
        public String PropertiesFile
        {
            get { return (this._propertiesFile); }
            set { this._propertiesFile = value; }
        }   
        
        #endregion

        /// <summary>
        /// Constructor. Reads the names of the input files and sets the maximum number
        /// of threads.
        /// </summary>
        /// <param name="qustionfile">File that contains the questions for the HITs (one per line)</param>
        /// <param name="propertiesfile">File that contains the properties of the HITs to create.</param>
        public CreateHitsMultiThreaded(String questionfile, String propertiesfile)
        {
            this.QuestionFile = questionfile;
            this.PropertiesFile = propertiesfile;
            
            // Limit the number of threads.                                 
            ThreadPool.SetMaxThreads(maxNumThreads, 1000);            
        }

        /// <summary>
        /// Reads the properties file and sets the properties needed by the HITs. Reads the
        /// question file. Starts creating the HITs by submitting a work item for each question
        /// in the question file to the .Net thread pool.
        /// </summary>
        public void Start()
        {
        	// Read the questions from the file
            questions = File.ReadAllLines(this.QuestionFile);
            numHits = questions.Length;
                           
            // Get the properties file and instantiate HIT model object from this file.
            string hitFile = Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, this.PropertiesFile);
            hitTemplate = client.DeserializeHIT(hitFile, MTurkSerializationFormat.Property);
            
            foreach (String question in questions)
            {
                // Enqueue the question string as a work item in the thread pool 
                // and pass in a callback method that gets invoked when a thread becomes 
                // available to do work.
                //
                // This can be replaced with other Amazon Mechanical Turk functionalities
                // like DisposeHIT to use the SDK in a multithreaded fashion.                
                if (!ThreadPool.QueueUserWorkItem(new WaitCallback(CreateHitCallback), question ))
                {
                    Console.WriteLine("Failed to enqueue work item for creation of HITt {0}", question);
                }
            }

            // Since this is a console app, we want to wait for all
            // work items to be completed before the process exits
            // In a web app or a windows service app, this would not
            // be necessary since it is always running.
            workCompleteEvent.WaitOne();
            Console.WriteLine("Finished. Press any key to exit ...");
        }

        /// <summary>
        /// Callback function that is called by the thread pool when a thread 
        /// becomes available
        /// </summary>
        /// <param name="state">The question we enqueued</param>
        private void CreateHitCallback(Object state)
        {        		
            String question = (String)state;
         
            try
            {
            	// Create a HIT and increment the success counter.
                HIT h = client.CreateHIT(hitTemplate.Title, hitTemplate.Description, hitTemplate.Reward.Amount, question, hitTemplate.MaxAssignments);
               	Interlocked.Increment(ref _successCount);
                Console.WriteLine("Successfully created HIT {0} for question '{1}' {2}", h.HITId, question, Progress);
            }
            catch (Exception ex)
            {
                Interlocked.Increment(ref _failureCount);
                Console.WriteLine("Failed to create hit {0}: {1}", question, ex.Message);
            }
            finally
            {
                // Decrement the "work-left" count and allow the
                // process to exit once it is zero.
                if (Interlocked.Decrement(ref numHits) == 0) 
                {
                    workCompleteEvent.Set();
                }
            }
        }
    }
}
