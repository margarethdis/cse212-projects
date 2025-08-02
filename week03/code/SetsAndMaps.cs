using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.Json;

public static class SetsAndMaps
{
    public static string[] FindPairs(string[] words)
    {
        var seen = new HashSet<string>();
        var result = new List<string>();

        foreach (string word in words)
        {
            if (word[0] == word[1]) continue;  // Ignorar pares con letras iguales

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

    // Clases necesarias para deserializar el JSON del USGS Earthquake data
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
