/* Author: DG-Mojaro
* 
* This program aims to calculate dice statistics and probabilities involving uncommon or unusual specifications.
* This includes factors that are not possible to calculate forulaically, such as the arbitrary 'dropping' of the lowest die in a set of rolled dice.
*/


using System;
using System.Security.Cryptography;
class Program
{
    static void Main(string[] args)
    {
        // Opening text menu
        Console.WriteLine("Welcome to the dice statistic calculator");
        
        // Variable initialisation
        bool confirmation = false;
        int userInputDieType = new();
        int userInputDieNumber = new();

        // While loop to serve as menu and confirm user input
        while (!confirmation)
        {
            Console.WriteLine("Please enter the number of faces on the die you'd like to calculate:");

            // Calling the function to get sanitised user input for the type of die
            userInputDieType = GetValidatedInteger(2, 100);

            Console.Clear();
            Console.WriteLine("Please enter the total number of dice you'd like to calculate:");

            // Calling the function to get sanitised user input for the number of dice
            userInputDieNumber = GetValidatedInteger(2, 100);

            // Confirms user input
            Console.Clear();
            Console.WriteLine("You have selected to roll " + userInputDieNumber + " d" + userInputDieType + "s");
            Console.WriteLine("Is this correct? (y/n)");

            // Checks for input confirmation with a (y/n) statement
            var ynConfirm = Console.ReadLine();
            if (ynConfirm == "y" | ynConfirm == "Y")
            {
                confirmation = true;
            }
            else
            {
                Console.WriteLine("Returning to input entry");
                Console.ReadLine();
                Console.Clear();
            }
        }

        // Creates a 'Dice' object 
        Dice dice = new Dice(userInputDieType);

        // Initialises the list of arrays containing the set of dice to roll
        List<int[]> allDiceToRoll = new();

        // Adds the required number of dice to the set of dice to roll
        int totalCombinations = 1;
        for (int i = 0; i < userInputDieNumber; i++)
        {
            allDiceToRoll.Add(dice.Faces);
            totalCombinations *= dice.Faces.Length;
        }

        // Calls a function to create the cartesian product of the passed set of dice
        List<int[]> allPossibleRollCombinations = CartesianProduct(allDiceToRoll, totalCombinations);

        Console.WriteLine();
        Console.WriteLine("Data processed");

        int[] rollTotals = new int[allPossibleRollCombinations.Count];

        // Outputs the dice combinations to the console
        Dictionary<int, int> rollTotalStats = new Dictionary<int, int>();
        int index = 0;
        foreach (int[] combination in allPossibleRollCombinations)
        {
            rollTotals[index] = RemoveLowestNumberSum(combination);

            if (rollTotalStats.ContainsKey(rollTotals[index]))
            {
                rollTotalStats[rollTotals[index]]++;
            }
            else
            {
                rollTotalStats[rollTotals[index]] = 1;
            }

            
            index++;
        }

        double totalValue = 0;

        foreach (KeyValuePair<int, int> pair in rollTotalStats)
        {
            Console.WriteLine("The roll total of: " + pair.Key + " appeared " + pair.Value + " times.");
            totalValue += (pair.Key * pair.Value);
        }

        double averageScore = (totalValue / (double)totalCombinations);
        Console.WriteLine("The average score is: " + averageScore);

        double testAverage = ((averageScore * 7) / 6);
        Console.WriteLine("For 7 drop the lowest, the average is: " + testAverage);

        List<int[]> testToGo = new List<int[]>();
        int bigNum = 1;
        for (int i = 0; i < 7; i++)
        {
            testToGo.Add(rollTotals);
            bigNum *= rollTotals.Length;
        }


        List<int[]> big7combo = CartesianProduct(testToGo, bigNum);

    }


    // Function to sanitise user input, and ensure it is an integer between the two passed values
    static int GetValidatedInteger(int low, int high)
    {
        int userInput = 0;
        bool isValidInput = false;

        // While loop that will loop until a valid input is returned
        while (!isValidInput)
        {
            Console.WriteLine("This needs to be an integer between " + low + " and " +  high + ":");

            var input = Console.ReadLine();

            // If statement to validate the input
            if (int.TryParse(input, out userInput))
            {
                if (userInput >= low && userInput <= high)
                {
                    isValidInput = true;
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("Error: Please enter a valid integer between 2 and 100");
                }
            }
            else
            {
                Console.Clear();
                Console.WriteLine("Error: Please enter a valid integer between 2 and 100");
            }
        }

        return userInput;
    }

    // Function to remove the lowest number from an array of integers
    static int RemoveLowestNumberSum(int[] numbers)
    {
        if (numbers.Length == 0)
        {
            return 0; // Return the original array if it's empty
        }

        // Sort the array in descending order
        Array.Sort(numbers);
        Array.Reverse(numbers);

        // Create a new array excluding the last element (the lowest number in the original array)
        int[] result = new int[numbers.Length - 1];
        Array.Copy(numbers, result, result.Length);

        int sum = result.Sum();

        return sum;
    }

    // Function to create the cartesian product of a passed list of integer arrays
    static List<int[]> CartesianProduct(List<int[]> allDicetoRoll, int totalCombinations)
    {
        // Initialises the list that will be returned, as well as the array to store the current combination
        List<int[]> result = new List<int[]>();
        int[] currentCombination = new int[allDicetoRoll.Count];

        // Calls the recursive function to create the cartesian product
        GenerateCombinationsRecursive(allDicetoRoll, 0, currentCombination, result, totalCombinations);

        return result;
    }

    // Function to recursively cycle through the sets of input dice to add every possible combination to the returned list
    static void GenerateCombinationsRecursive(List<int[]> allDiceToRoll, int index, int[] currentCombination, List<int[]> result, int totalCombinations)
    {
        // Escapes the function if the index has reached the end of the current combination array
        if (index == allDiceToRoll.Count)
        {
            result.Add((int[])currentCombination.Clone());       
            
            // Calculates and outputs the current percentage completion. Can comment out below to speed up processing, but at the cost of verbose output.
            Console.Write($"\rProgress: {(((double)result.Count / totalCombinations) * 100):F2}%");
            return;
        }

        // Calls the function again for each value in the array at the specified index
        foreach (var value in allDiceToRoll[index])
        {
            // Adds the value of the current die face to the current combination array
            currentCombination[index] = value;

            // Calls the function again, adding 1 to the index
            GenerateCombinationsRecursive(allDiceToRoll, index + 1, currentCombination, result, totalCombinations);
        }
    }



}