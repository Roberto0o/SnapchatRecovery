using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

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
                Console.WriteLine("Invalid directory. Exiting.");
                return;
            }

            // Determine the name for the new directories
            string sourceDirName = new DirectoryInfo(sourceDir).Name;
            string parentDir = Directory.GetParent(sourceDir)?.FullName;
            if (string.IsNullOrEmpty(parentDir))
            {
                Console.WriteLine("Cannot determine the parent directory. Exiting.");
                return;
            }
            string formattedDir = Path.Combine(parentDir, sourceDirName + "_formatted");
            string discardedDir = Path.Combine(parentDir, sourceDirName + "_discarded");

            Directory.CreateDirectory(formattedDir);
            Directory.CreateDirectory(discardedDir);

            Console.WriteLine("Fetching files...");

            // Get all files in the directory excluding unwanted patterns
            string[] allFiles = Directory.GetFiles(sourceDir)
                .Where(file => !file.Contains("overlay~Snapchat") && !file.Contains("metadata") && !file.EndsWith("overlay"))
                .ToArray();

            // Get all discarded files
            string[] discardedFiles = Directory.GetFiles(sourceDir)
                .Where(file => file.Contains("overlay~Snapchat") || file.Contains("metadata") || file.EndsWith("overlay"))
                .ToArray();

            int totalFiles = allFiles.Length + discardedFiles.Length;

            if (totalFiles == 0)
            {
                Console.WriteLine("No files found in the directory after filtering. Exiting.");
                return;
            }

            Console.WriteLine($"Found {totalFiles} files. Processing...");

            int fileIndex = 0;

            // Copy and log discarded files
            foreach (string filePath in discardedFiles)
            {
                fileIndex++;
                string fileName = Path.GetFileName(filePath);

                try
                {
                    Console.WriteLine($"Copying file: {fileName} ({fileIndex}/{totalFiles})");
                    string newFilePath = Path.Combine(discardedDir, fileName);
                    File.Copy(filePath, newFilePath, true);
                    Console.WriteLine($"Copied file: {fileName} ({fileIndex}/{totalFiles})");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error copying file: {fileName} ({fileIndex}/{totalFiles}): {ex.Message}");
                }
            }

            // Process remaining files
            foreach (string filePath in allFiles)
            {
                fileIndex++;
                string fileName = Path.GetFileName(filePath);

                try
                {
                    // Extract date from the file name using regex
                    Regex dateRegex = new Regex(@"^(\d{4}-\d{2}-\d{2})_");
                    Match match = dateRegex.Match(fileName);

                    if (!match.Success)
                    {
                        Console.WriteLine($"Skipping file: {fileName} ({fileIndex}/{totalFiles}) (No valid date found)");
                        continue;
                    }

                    // Parse the date and set time to 12:00:00
                    DateTime extractedDate = DateTime.Parse(match.Groups[1].Value).Date.AddHours(12);

                    Console.WriteLine($"Processing file: {fileName} ({fileIndex}/{totalFiles})");

                    // Rename the file: remove date and replace "main" with "snapmem"
                    string newFileName = Regex.Replace(fileName.Substring(match.Length), "main", "snapmem");
                    string newFilePath = Path.Combine(formattedDir, newFileName);

                    // Create a copy of the file in the formatted directory
                    File.Copy(filePath, newFilePath, true);

                    // Set the extracted date as both creation and last write times
                    File.SetCreationTime(newFilePath, extractedDate);
                    File.SetLastWriteTime(newFilePath, extractedDate);

                    Console.WriteLine($"Processed file: {fileName} ({fileIndex}/{totalFiles})");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error processing file: {fileName} ({fileIndex}/{totalFiles}): {ex.Message}");
                }
            }

            Console.WriteLine("Processing complete. All files saved in the respective directories.");

            // Prompt the user to close the console
            Console.WriteLine("Press 'Y' to close the console, or any other key to keep it open.");
            var key = Console.ReadKey().Key;
            if (key == ConsoleKey.Y)
            {
                return;
            }
        }
    }
}