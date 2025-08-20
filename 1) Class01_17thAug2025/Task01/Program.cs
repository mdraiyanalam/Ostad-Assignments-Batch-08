using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArrayTasks
{
    class Program
    {
        static void Main(string[] args)
        {
            // Task 2: Integer array of size 5 with console input
            int[] numbers = new int[5];

            Console.WriteLine("Enter 5 integer numbers:");
            for (int i = 0; i < 5; i++)
            {
                Console.Write($"Number {i + 1}: ");
                numbers[i] = Convert.ToInt32(Console.ReadLine());
            }

            Console.WriteLine("\nYou entered these numbers:");
            foreach (int num in numbers)
            {
                Console.Write(num + " ");
            }

        }
    }
}