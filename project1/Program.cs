using System;
using System.Collections.Generic;


namespace project1
{
    class Program
    {
        static void Main(string[] args)
        {
            /*
            Console.WriteLine("Welcome to the 2019 NFL Draft.");
            Console.WriteLine("This simple program will help you to make your selections within the confines of your salary cap.");
            Console.WriteLine("Please press any key to begin");
            Console.ReadLine();
            */
        }
    }
    class Player
    {
        public string Position { get; set; }
        public string Rank { get; set; }
        public string Name { get; set; }
        public string College { get; set; }
        public double requestedSalary { get; set; }
        public bool playerSelected { get; set; }
        
    }
}


