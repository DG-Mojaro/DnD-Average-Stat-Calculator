using System;
class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Welcome to the dice statistic calculator");

        int userInput = GetValidatedInput();

        Console.Clear();
        Console.WriteLine("You entered a valid input number: " + userInput);

        Dice testDice = new Dice(userInput);

        var allRolls = GenerateAllRolls(testDice, 4);

        int[] rollsTotal = new int[allRolls.Count];

        int i = 0;
        foreach (var roll in allRolls)
        {
            rollsTotal[i] = RemoveLowestNumberSum(roll);
            Console.WriteLine(string.Join(", ", roll));
            Console.WriteLine(rollsTotal[i]);

            i++;
        }



    }

    static List<int[]> GenerateAllRolls(Dice dice, int numberOfDice)
    {
        IEnumerable<int> diceFaces = Enumerable.Range(1, dice.NumberOfFaces);

        IEnumerable<IEnumerable<int>> rolls = CartesianProduct(Enumerable.Repeat(diceFaces, numberOfDice));

        return rolls.Select(r => r.ToArray()).ToList();
    }

    static IEnumerable<IEnumerable<T>> CartesianProduct<T>(IEnumerable<IEnumerable<T>> sequences)
    {
        IEnumerable<IEnumerable<T>> result = new[] { Enumerable.Empty<T>() };
        foreach (var sequence in sequences)
        {
            result = from accseq in result
                     from item in sequence
                     select accseq.Concat(new[] { item });
        }

        return result;
    }



    static int GetValidatedInput()
    {
        int userInput = 0;
        bool isValidInput = false;

        while (!isValidInput)
        {
            Console.Clear();
            Console.WriteLine("Please enter the number of faces on the die you'd like to calculate.");
            Console.WriteLine("This needs to be an integer between 2 and 100:");

            string input = Console.ReadLine();

            if (int.TryParse(input, out userInput))
            {
                if (userInput >= 2 && userInput <= 100)
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

    /*
    static List<int> GeneratePossibleRolls(Dice dice, int numberOfDice, int numberOfDiceToDrop, int currentIndex)
    {
        List<int>[] possibleRolls = new List<int>[(dice.NumberOfFaces ^ numberOfDice)];

        foreach (List<int> i in possibleRolls)
        {
            foreach (int j in possibleRolls[i])
            {

            }
        }

        return possibleRolls;
    }
    */



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
}



