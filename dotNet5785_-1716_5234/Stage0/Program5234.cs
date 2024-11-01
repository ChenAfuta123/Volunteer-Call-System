// See https://aka.ms/new-console-template for more information
using System;
namespace stage0
{
    partial class Program
    {
        static partial void Welcome1716();
        static void Main(String[] args)
        {
            Welcome5234();
            Welcome1716();
            Console.ReadKey();
        }
       
        private static void Welcome5234()
        {
            Console.WriteLine("Enter your name:");
            string name = Console.ReadLine();
            Console.WriteLine("{0}, welcome to my first console application", name);
        }
        //static partial void Welcome1716();
    }
}

