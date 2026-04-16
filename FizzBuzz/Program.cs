using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyFirstDotNetApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string input = Console.ReadLine()!;
            string[] split_input = input.Split(' ');

            // Parse the input. I assume that the input is correct here.
            int x = int.Parse(split_input[0]);
            int y = int.Parse(split_input[1]);
            int n = int.Parse(split_input[2]);

            // There are two ways in my opinion to implememnt FizzBuzz.
            // The first is a case/switch/if_else_chain covering the four cases.
            // The second way is to have 3 if statements like what I've written below.
            for (int i = 1; i <= n; i++)
            {
                if (i % x == 0)
                {
                    Console.Write("Fizz");
                }
                if (i % y == 0)
                {
                    Console.Write("Buzz");
                }
                if (i % x != 0 && i % y != 0)
                {
                    Console.Write(i);
                }
                Console.WriteLine();
            }
        }
    }
}