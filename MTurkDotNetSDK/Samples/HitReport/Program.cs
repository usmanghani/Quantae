#region Copyright & license notice
/*
 * Copyright: Copyright (c) 2007 Amazon Technologies, Inc.
 * License:   Apache License, Version 2.0
 */
#endregion

using System;
using System.Collections.Generic;
using System.Text;

namespace HitReport
{
    class Program
    {
        static void Main(string[] args)
        {
            HitReport report = new HitReport();
            report.PrintAllHits();

            Console.Write("Press 'Enter' to end sample ...");
            Console.ReadLine();
        }
    }
}
