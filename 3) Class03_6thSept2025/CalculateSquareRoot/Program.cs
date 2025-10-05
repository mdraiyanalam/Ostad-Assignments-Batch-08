using System;

public class SquareRootTest
{
    public static double CalculateSquareRoot(double number)
    {
        try
        {
            if (number < 0)
            {
                throw new ArgumentException();
            }
            return Math.Round(Math.Sqrt(number), 4);
        }
        catch (ArgumentException)
        {
            Console.WriteLine("Error: Cannot calculate square root of a negative number!");
            return -1;
        }
    }

    public static void Main()
    {
        Console.WriteLine("Type END to exit the program.");
        while (true)
        {
            Console.Write("Enter a number: ");
            string inputString = Console.ReadLine();

            if (inputString.ToLower() == "end")
            {
                break;
            }

            double input = Convert.ToDouble(inputString);
            double result = CalculateSquareRoot(input);
            Console.WriteLine($"Result: {result}");
        }
    }
}