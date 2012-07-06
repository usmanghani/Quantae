
#region Copyright & license notice
/*
 * Copyright: Copyright (c) 2008 Amazon Technologies, Inc.
 * License:   Apache License, Version 2.0
 */
#endregion

using System;
using System.Collections.Generic;
using System.Text;

namespace CreateHitsMultiThreaded
{
    class Program
    {
        /// <summary>
        /// Creates HITs using multithreading.  
        /// </summary>
        /// <param name="args">
        /// The path to the file that contains the questions for the HITs and
        /// the path to the file that contains the properties for the HITs.
        /// </param>
        static void Main(string[] args)
        {
            try
            {
            	
            	CreateHitsMultiThreaded createHitsApp = new CreateHitsMultiThreaded(args[0], args[1]);
                createHitsApp.Start();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to create HITs: {0}", ex.Message);
            }
            finally
            {
                Console.ReadLine();
            }
        }
    }
}
