// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

// Task 3: Integer array of size 3 with random numbers
int[] randomNumbers = new int[3];
Random random = new Random();

// Insert 3 random numbers between 0 - 999
for (int i = 0; i < 3; i++){
    randomNumbers[i] = random.Next(0, 999);
}

Console.WriteLine("\n\nRandom numbers generated:");
foreach (int num in randomNumbers){
    Console.Write(num + " ");
}

// Find largest number using if-else
int largest = randomNumbers[0];
if (randomNumbers[1] > largest){
    largest = randomNumbers[1];
}
if (randomNumbers[2] > largest){
    largest = randomNumbers[2];
}
Console.WriteLine($"\nLargest number: {largest}");

// Find smallest number using switch
int smallest = randomNumbers[0];
for (int i = 1; i < 3; i++){
    switch (randomNumbers[i] < smallest){
        case true:
            smallest = randomNumbers[i];
            break;
        case false:
            // Do nothing
            break;
    }
}
Console.WriteLine($"Smallest number: {smallest}");

Console.WriteLine("\nPress any key to exit...");
Console.ReadKey();
