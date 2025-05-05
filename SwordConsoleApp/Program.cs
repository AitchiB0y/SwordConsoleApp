using System;
using System.Collections.Generic;


namespace SwordConsoleApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            if (!ValidateArguments(args, out string filePath, out bool ignoreCase))
            {
                return;
            }

            AnalyseInputFile analyseInputFile = new(filePath, ignoreCase);
            // Call the method to analyze character frequency
            if (analyseInputFile.AnalyseInputFileCharacterFrequency())
                Console.WriteLine("Analysis completed successfully.");
            else
                Console.WriteLine("Analysis failed.");
        }

        /// <summary>
        /// Validates the command line arguments.
        /// </summary>
        /// <param name="args"></param>
        /// <param name="filePath"></param>
        /// <param name="ignoreCase"></param>
        /// <returns></returns>
        static bool ValidateArguments(string[] args, out string filePath, out bool ignoreCase)
        { //this could be refactored to use custom validation logic using the FluentValidation package.
            //it could also be moved to the AnalyseInputFile class to make it completely self contained.
            filePath = string.Empty;
            ignoreCase = false;

            if (args.Length < 1)
            {
                Console.WriteLine("No input file specified. Usage: SwordConsoleApp.exe <file-path> --ignore-case");
                return false;
            }

            filePath = args[0];
            ignoreCase = args.Length > 1 && args[1].Equals("--ignore-case", StringComparison.OrdinalIgnoreCase);

            if (!CheckFileExists(filePath))
            {
                Console.WriteLine($"Error: File '{filePath}' not found.");
                return false;
            }

            return true;
        }
        /// <summary>
        /// Checks if the specified file exists.
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        static bool CheckFileExists(string filePath)
        {
            return File.Exists(filePath);
        }

    }
}