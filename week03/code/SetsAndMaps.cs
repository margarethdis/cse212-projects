using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.Json;

public static class SetsAndMaps
{
    // Ejercicio 1: Encuentra pares invertidos
    public static string[] FindPairs(string[] words)
    {
        var seen = new HashSet<string>();
        var result = new List<string>();

        foreach (string word in words)
        {
            if (word[0] == word[1]) continue;

            string reversed = new string(word.Reverse().ToArray());

            if (seen.Contains(reversed))
            {
                var pair = new List<string> { word, reversed };
                pair.Sort();
                result.Add($"{pair[0]} & {pair[1]}");
            }

            seen.Add(word);
        }

        return result.ToArray();
    }

    // Ejercicio 2: Resume grados desde archivo census.txt
    public static Dictionary<string, int> SummarizeDegrees(string filename)
    {
        var degrees = new Dictionary<string, int>();

        foreach (var line in File.ReadLines(filename))
        {
            var fields = line.Split(',');

            if (fields.Length > 3)
            {
                string degree = fields[3].Trim();

                if (degrees.ContainsKey(degree))
                    degrees[degree]++;
                else
                    degrees[degree] = 1;
            }
        }

        return degrees;
    }

    // Ejercicio 3: Verifica si son anagramas
    public static bool IsAnagram(string word1, string word2)
    {
        word1 = word1.Replace(" ", "").ToLower();
        word2 = word2.Replace(" ", "").ToLower();

        if (word1.Length != word2.Length) return false;

        var letterCounts = new Dictionary<char, int>();

        foreach (char c in word1)
        {
            if (letterCounts.ContainsKey(c))
                letterCounts[c]++;
            else
                letterCounts[c] = 1;
        }

        foreach (char c in word2)
        {
            if (!letterCounts.ContainsKey(c)) return false;
            letterCounts[c]--;
            if (letterCounts[c] < 0) return false;
        }

        return true;
    }

    // Ejercicio 4: Resuelve un laberinto BFS (anchura)
    public static string MazeSolver(string[] maze)
    {
        int rows = maze.Length;
        int cols = maze[0].Length;

        (int r, int c)? start = null, end = null;

        for (int r = 0; r < rows; r++)
        {
            for (int c = 0; c < cols; c++)
            {
                if (maze[r][c] == 'S') start = (r, c);
                if (maze[r][c] == 'E') end = (r, c);
            }
        }

        if (start == null || end == null) return "Invalid maze";

        var queue = new Queue<(int r, int c, string path)>();
        var visited = new HashSet<(int, int)>();
        var directions = new (int dr, int dc, char dir)[]
        {
            (-1, 0, 'U'), (1, 0, 'D'), (0, -1, 'L'), (0, 1, 'R')
        };

        queue.Enqueue((start.Value.r, start.Value.c, ""));
        visited.Add((start.Value.r, start.Value.c));

        while (queue.Count > 0)
        {
            var (r, c, path) = queue.Dequeue();

            if ((r, c) == end.Value) return path;

            foreach (var (dr, dc, dir) in directions)
            {
                int nr = r + dr;
                int nc = c + dc;

                if (nr >= 0 && nr < rows && nc >= 0 && nc < cols &&
                    maze[nr][nc] != '#' && !visited.Contains((nr, nc)))
                {
                    visited.Add((nr, nc));
                    queue.Enqueue((nr, nc, path + dir));
                }
            }
        }

        return "No path found";
    }

    // Extra: Earthquake summary si quieres usarlo para probar después
    public class FeatureCollection
    {
        public List<Feature> Features { get; set; }
    }

    public class Feature
    {
        public Properties Properties { get; set; }
    }

    public class Properties
    {
        public string Place { get; set; }
        public double Mag { get; set; }
    }

    public static string[] EarthquakeDailySummary()
    {
        const string uri = "https://earthquake.usgs.gov/earthquakes/feed/v1.0/summary/all_day.geojson";
        using var client = new HttpClient();
        using var getRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);
        using var jsonStream = client.Send(getRequestMessage).Content.ReadAsStream();
        using var reader = new StreamReader(jsonStream);
        var json = reader.ReadToEnd();
        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

        var featureCollection = JsonSerializer.Deserialize<FeatureCollection>(json, options);

        var results = new List<string>();

        if (featureCollection?.Features != null)
        {
            foreach (var feature in featureCollection.Features)
            {
                string place = feature.Properties.Place;
                double mag = feature.Properties.Mag;
                results.Add($"{place} - Mag {mag}");
            }
        }

        return results.ToArray();
    }
}
