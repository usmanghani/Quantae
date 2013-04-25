using System;
using System.Collections.Generic;
using System.Text;

namespace ReviewPolicySample
{
    class Program
    {
        static void Main(string[] args)
        {
            MTurkReviewPolicy sample = new MTurkReviewPolicy();
            if (sample.HasEnoughFunds())
            {
                sample.CreateHITWithReviewPolicy();
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
