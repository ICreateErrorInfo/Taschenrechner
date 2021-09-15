using System;
using System.Collections.Generic;

namespace Rechner
{
    class Program
    {
        static void Main(string[] args)
        {
            Rechner rechner= new Rechner();
            while (true)
            {
                string input = Console.ReadLine();
                if(input == "Clear")
                {
                    Console.Clear();
                }
                else
                {
                    rechner.Calculate(input);
                    Console.WriteLine(rechner.ergebnis);
                    if (rechner.isEquation)
                    {
                        Console.WriteLine(rechner.equals);
                    }
                }
            }
        }       
    }
}
