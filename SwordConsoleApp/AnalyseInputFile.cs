using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace SwordConsoleApp
{
    public class AnalyseInputFile
    {
        private string _filePath;
        private bool _ignoreCase;
        public AnalyseInputFile(string filePath, bool ignoreCase)
        {
            _filePath = filePath;
            _ignoreCase = ignoreCase;
        }
        /// <summary>
        /// Reads the input file and analyzes the frequency of characters.
        /// </summary>
        /// <returns></returns>
        public bool AnalyseInputFileCharacterFrequency()
        {
            try
            {
                string content = File.ReadAllText(_filePath);

                // Analyze character frequency
                var result = AnalyseCharacterFrequency(content, _ignoreCase);

                // Output results
                Console.WriteLine($"Total characters: {result.TotalCharacters}");
                foreach (var entry in result.TopCharacters)
                {
                    Console.WriteLine($"{entry.Character} ({entry.Count})");
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Analyzes the frequency of characters in a given string using LINQ.
        /// </summary>
        /// <param name="content">The input string.</param>
        /// <param name="ignoreCase">Whether to ignore case when counting characters.</param>
        /// <returns>A tuple containing the total number of characters and a list of the top 10 most frequent characters.</returns>
        internal static (int TotalCharacters, List<(char Character, int Count)> TopCharacters)
            AnalyseCharacterFrequency(string content, bool ignoreCase)
        {
            // Define characters to ignore
            char[] whiteSpaceChars = { ' ', '\r', '\n', '\t' };

            // Normalize case if needed
            if (ignoreCase)
            {
                content = content.ToLower();
            }

            // Filter out whitespace characters
            var filteredContent = content.Where(c => !whiteSpaceChars.Contains(c));

            // Count character frequencies
            var frequency = filteredContent
                .GroupBy(c => c)
                .Select(group => new { Character = group.Key, Count = group.Count() })
                .OrderByDescending(x => x.Count)
                .ThenBy(x => x.Character)
                .ToList();

            // Get total characters and top 10 most frequent characters
            int totalCharacters = frequency.Sum(x => x.Count);
            var topCharacters = frequency
                .Take(10)
                .Select(x => (x.Character, x.Count))
                .ToList();

            return (totalCharacters, topCharacters);
        }
    }
}
