// See https://aka.ms/new-console-template for more information
//Console.WriteLine("Hello, World!");

using System;

namespace CarApp
{
    // Car class definition
    public class Car
    {
        // Properties
        public string Brand { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
        public int Mileage { get; set; }

        // Constructor
        public Car(string brand, string model, int year, int mileage = 0)
        {
            Brand = brand;
            Model = model;
            Year = year;
            Mileage = mileage;
        }

        // Task 2: Drive method
        public void Drive(int kilometers)
        {
            // Increase mileage by the given kilometers
            Mileage += kilometers;

            // Print the driving information
            Console.WriteLine($"Car {Brand} {Model} has driven {kilometers} km. Total mileage: {Mileage} km.");
        }

        // Task 3: ShowCarInfo method
        public void ShowCarInfo()
        {
            Console.WriteLine($"Car Info: Brand - {Brand}, Model - {Model}, Year - {Year}, Mileage - {Mileage} km");
        }

        // Optional: Override ToString method for better display
        public override string ToString()
        {
            return $"{Year} {Brand} {Model} - {Mileage} km";
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");
            Console.WriteLine("=== Car Class Demonstration ===\n");

            // Task 2: Create two different Car objects
            Car toyota = new Car("Toyota", "Corolla", 2022, 0);
            Car tesla = new Car("Tesla", "Model 3", 2023, 0);

            // Show initial car info
            Console.WriteLine("Initial Car Information:");
            toyota.ShowCarInfo();
            tesla.ShowCarInfo();
            Console.WriteLine();

            // Task 2: Call Drive method on both cars with different kilometers
            Console.WriteLine("Driving the cars:");
            toyota.Drive(50);     // Toyota drives 50 km
            tesla.Drive(120);     // Tesla drives 120 km
            toyota.Drive(30);     // Toyota drives another 30 km

            Console.WriteLine();

            // Show final car info
            Console.WriteLine("Final Car Information:");
            toyota.ShowCarInfo();
            tesla.ShowCarInfo();

            // Keep console window open
            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
        }
    }
}
