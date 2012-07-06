#region Copyright & license notice
/*
 * Copyright: Copyright (c) 2007 Amazon Technologies, Inc.
 * License:   Apache License, Version 2.0
 */
#endregion

using System;
using System.Collections.Generic;
using System.Text;

namespace Reviewer
{
    public class Program
    {
        static void Main(string[] args)
        {
            string hitID = null;
            if (args.Length > 0)
            {
                hitID = args[0];
            }
            Reviewer rev = new Reviewer();
            rev.ReviewAnswers(hitID);

            Console.Write("Press 'Enter' to end sample ...");
            Console.ReadLine();
        }
    }
}
