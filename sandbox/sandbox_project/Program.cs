
using System;

public class Program
{
    static void Main(string[] args)
    {
        List<string> palabras = new List<string> { "dog", "cat", "fish" };
        DoSomethingElse(palabras);

    }

    static void DoSomethingElse(List<string> words)
    {
        for (int i = 0; i < words.Count; i++)
        {
            for (int j = 0; j < words.Count; j++)
            {
                Console.WriteLine($"{words[i]} - {words[j]}");

            }
        }
    }
}