#region Copyright & license notice
/*
 * Copyright: Copyright (c) 2007 Amazon Technologies, Inc.
 * License:   Apache License, Version 2.0
 */
#endregion

using System;
using System.Collections.Generic;
using System.Text;


namespace BestImage
{
    class Program
    {
        static void Main(string[] args)
        {
            MTurkBestImage img = new MTurkBestImage();
            if (img.HasEnoughFunds())
            {
                img.CreateBestImage();
            }
            else
            {
                Console.WriteLine("You do not have enough funds to run this sample");
            }

            Console.Write("Press 'Enter' to end sample ...");
            Console.ReadLine();
        }
    }
}
