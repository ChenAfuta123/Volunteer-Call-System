// See https://aka.ms/new-console-template for more information
using System;
namespace stage0
{
    partial class Program
    {
        static void Main(String[] args)
        {
            welcome5234();
            welcome1716();
            Console.ReadKey();
        }

        private static void welcome5234()
        {
            Console.WriteLine("Enter your name:");
            string name = Console.ReadLine();
            Console.WriteLine("{0}, welcome to my first console application", name);
        }
        static partial void welcome1716();
    }
}

