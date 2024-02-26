
using System;
using System.Collections.Generic;
using System.IO;

class Program
{
    static void Main()
    {
        // Step 1: Read the text file line by line and store them in a HashMap
        Dictionary<int, string> linesHashMap = new Dictionary<int, string>();
        string filePath = "your_text_file.txt";

        if (!File.Exists(filePath))
        {
            Console.WriteLine("ERROR: テキストファイルが見つかりませんでした。");
            return; // Exit the program if the text file does not exist
        }

        try
        {
            using (StreamReader reader = new StreamReader(filePath))
            {
                int lineNumber = 1;
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    linesHashMap[lineNumber] = line;
                    lineNumber++;
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while reading the text file: {ex.Message}");
            return;
        }

        // Step 2: Search all the files in the directory in the system and look for a match which matches to the line which is read from the text file
        string sourceDirectory = "source_directory";
        if (!Directory.Exists(sourceDirectory))
        {
            Console.WriteLine("ERROR: ソースディレクトリが見つかりませんでした。");
            return;
        }

        List<string> matchedFilesAndDirs = new List<string>();
        TraverseDirectory(sourceDirectory, linesHashMap, matchedFilesAndDirs);

        // Step 3: Write matched file paths along with timestamp and row number to a log file
        string logFilePath = "log_file.txt";
        try
        {
            using (StreamWriter writer = new StreamWriter(logFilePath, true)) // Append mode
            {
                foreach (string matchedFile in matchedFilesAndDirs)
                {
                    string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    string fullPath = Path.GetFullPath(matchedFile);
                    int lineNumber = GetLineNumber(linesHashMap, matchedFile);
                    writer.WriteLine($"{timestamp}: {fullPath} (行番号: {lineNumber})");
                }
            }

            Console.WriteLine("Matched file paths along with timestamp and row number have been written to the log file.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while writing to the log file: {ex.Message}");
        }

        // Continue with the rest of the program...
    }

    
    static void TraverseDirectory(string directory, Dictionary<int, string> linesHashMap, List<string> matchedFilesAndDirs)
    {
        foreach (string path in Directory.GetFileSystemEntries(directory))
        {
            if (File.Exists(path) || Directory.Exists(path))
            {
                foreach (KeyValuePair<int, string> kvp in linesHashMap)
                {
                    if (path.Contains(kvp.Value))
                    {
                        matchedFilesAndDirs.Add(path);
                        break;
                    }
                }
            }

            if (Directory.Exists(path))
            {
                TraverseDirectory(path, linesHashMap, matchedFilesAndDirs);
            }
        }
    }

    
    static int GetLineNumber(Dictionary<int, string> linesHashMap, string filePath)
    {
        foreach (KeyValuePair<int, string> kvp in linesHashMap)
        {
            if (filePath.Contains(kvp.Value))
            {
                return kvp.Key;
            }
        }
        return -1; // Line number not found
    }
}
