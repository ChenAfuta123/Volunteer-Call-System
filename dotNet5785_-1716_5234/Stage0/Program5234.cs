// See https://aka.ms/new-console-template for more information
//hi

using targil0;
namespace targil0
{
    partial class Program
    {
       
        static void Main(string[] args)
        {
            Welcome5234();
            Welcome1716();
            Console.ReadKey();
        }

        static partial void Welcome1716();
        private static void Welcome5234()
        {
            Console.WriteLine("Enter your name:");
            string name = Console.ReadLine();
            Console.WriteLine("{0}, welcome to my first console application", name);
        }
        


    }
  
}

