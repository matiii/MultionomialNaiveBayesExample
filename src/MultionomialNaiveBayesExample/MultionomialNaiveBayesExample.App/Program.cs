using System;

namespace MultionomialNaiveBayesExample.App
{
    class Program
    {
        static void Main(string[] args)
        {
            var example = new Example();
            example.Show();

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}
