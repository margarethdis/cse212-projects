using System;

class Program
{
    static void Main()
    {
        string[] words = { "am", "at", "ma", "if", "fi" };
        var pairs = SetsAndMaps.FindPairs(words);

        Console.WriteLine("Pares encontrados:");
        foreach (var pair in pairs)
        {
            Console.WriteLine(pair);
        }
    }
}
