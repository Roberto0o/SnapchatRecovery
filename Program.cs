using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace SnapchatRecovery
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Enter the directory path:");
            string sourceDir = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(sourceDir) || !Directory.Exists(sourceDir))
            {
                Console.WriteLine("Invalid directory path. Exiting program.");
                return;
            }

            string formattedDir = Path.Combine(Directory.GetParent(sourceDir).FullName,
                Path.GetFileName(sourceDir) + "_formatted");
            string discardedDir = Path.Combine(Directory.GetParent(sourceDir).FullName,
                Path.GetFileName(sourceDir) + "_discarded");

            Directory.CreateDirectory(formattedDir);
            Directory.CreateDirectory(discardedDir);

            string[] allFiles = Directory.GetFiles(sourceDir);
            List<string> discardedPatterns = new List<string> { "metadata", "overlay~Snapchat", "overlay" };

            int totalFiles = allFiles.Length;
            int currentFileIndex = 0;

            foreach (string file in allFiles)
            {
                currentFileIndex++;
                string fileName = Path.GetFileName(file);

                // Check if the file should be discarded
                if (discardedPatterns.Any(pattern => fileName.Contains(pattern)) || fileName.EndsWith("overlay"))
                {
                    Console.WriteLine($"Skipping file: {fileName} ({currentFileIndex}/{totalFiles})");
                    string discardedFilePath = Path.Combine(discardedDir, fileName);
                    File.Copy(file, discardedFilePath, true);
                    Console.WriteLine($"Copied file: {fileName} ({currentFileIndex}/{totalFiles})");
                    continue;
                }

                // Extract the date from the file name
                DateTime fileDate;
                if (!TryExtractDateFromFileName(fileName, out fileDate))
                {
                    Console.WriteLine($"Skipping file: {fileName} ({currentFileIndex}/{totalFiles}) (No valid date found)");
                    // Copy skipped files to discarded directory
                    string discardedFilePath = Path.Combine(discardedDir, fileName);
                    File.Copy(file, discardedFilePath, true);
                    Console.WriteLine($"Copied file: {fileName} ({currentFileIndex}/{totalFiles})");
                    continue;
                }

                Console.WriteLine($"Processing file: {fileName} ({currentFileIndex}/{totalFiles})");

                // Set the time to 12:00:00
                fileDate = fileDate.Date.Add(new TimeSpan(12, 0, 0));

                // Rename the file
                string newFileName = RenameFile(fileName);
                string newFilePath = Path.Combine(formattedDir, newFileName);

                File.Copy(file, newFilePath, true);

                // Adjust the file properties
                File.SetCreationTime(newFilePath, fileDate);
                File.SetLastWriteTime(newFilePath, fileDate);

                Console.WriteLine($"Processed file: {fileName} ({currentFileIndex}/{totalFiles})");
            }

            Console.WriteLine("Processing complete. All files saved in the respective directories.");
            Console.WriteLine("Press 'Y' to close the console, or any other key to keep it open.");

            var key = Console.ReadKey();
            if (key.Key == ConsoleKey.Y)
            {
                Environment.Exit(0);
            }
        }

        static bool TryExtractDateFromFileName(string fileName, out DateTime fileDate)
        {
            fileDate = default;
            string[] parts = fileName.Split('_');

            if (parts.Length > 0 && DateTime.TryParseExact(parts[0], "yyyy-MM-dd",
                CultureInfo.InvariantCulture, DateTimeStyles.None, out fileDate))
            {
                return true;
            }

            return false;
        }

        static string RenameFile(string fileName)
        {
            string[] parts = fileName.Split('_');

            if (parts.Length > 1)
            {
                // Remove the date and `_`
                string restOfFileName = string.Join("_", parts.Skip(1));

                // Replace "main" with "snapmem"
                restOfFileName = restOfFileName.Replace("main", "snapmem");

                return restOfFileName;
            }

            return fileName;
        }
    }
}
