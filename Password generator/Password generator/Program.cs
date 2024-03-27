using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

class Program
{
    static void Main()
    {
        // Number of passwords to generate
        int numberOfPasswords = 1;

        // Length of each password
        int passwordLength = 10;

        // Directory path to store passwords file
        string directoryPath = @"E:\school\passwordgenerator\PasswordGen\Password generator";

        // Ensure the directory exists
        Directory.CreateDirectory(directoryPath);

        // File path to store passwords
        string filePath = Path.Combine(directoryPath, "passwords.txt");

        // Generate passwords and corresponding site names
        string[] newPasswords = GeneratePasswords(numberOfPasswords, passwordLength);
        string[] siteNames = GetSiteNames(numberOfPasswords);

        // Combine site names and passwords
        string[] combinedEntries = CombineEntries(siteNames, newPasswords);

        // Write all entries to file
        try
        {
            // Remove existing entries for duplicated site names
            RemoveExistingEntries(filePath, siteNames);

            // Append new entries to file
            File.AppendAllLines(filePath, combinedEntries);
            Console.WriteLine("Passwords have been written to " + filePath);
        }
        catch (Exception ex)
        {
            Console.WriteLine("An error occurred: " + ex.Message);
        }
    }

    static string[] GeneratePasswords(int numberOfPasswords, int passwordLength)
    {
        // Characters to use for generating passwords
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*()";

        // Random number generator
        Random random = new Random();

        // Array to store generated passwords
        string[] passwords = new string[numberOfPasswords];

        for (int i = 0; i < numberOfPasswords; i++)
        {
            // StringBuilder to build the password
            StringBuilder passwordBuilder = new StringBuilder();

            // Generate password characters
            for (int j = 0; j < passwordLength; j++)
            {
                // Append a random character from the 'chars' string
                passwordBuilder.Append(chars[random.Next(chars.Length)]);
            }

            // Store the generated password
            passwords[i] = passwordBuilder.ToString();
        }

        return passwords;
    }

    static string[] GetSiteNames(int numberOfSites)
    {
        List<string> siteNames = new List<string>();

        for (int i = 0; i < numberOfSites; i++)
        {
            Console.Write("Enter the site name: ");
            string siteName = Console.ReadLine();

            // If the site name already exists, remove the previous entry
            if (siteNames.Contains(siteName))
            {
                int index = siteNames.IndexOf(siteName);
                siteNames.RemoveAt(index);
            }

            siteNames.Add(siteName);
        }

        return siteNames.ToArray();
    }

    static string[] CombineEntries(string[] siteNames, string[] passwords)
    {
        // Combine site names and passwords into formatted entries
        string[] combinedEntries = new string[siteNames.Length];

        for (int i = 0; i < siteNames.Length; i++)
        {
            combinedEntries[i] = siteNames[i] + ": " + passwords[i];
        }

        return combinedEntries;
    }

    static void RemoveExistingEntries(string filePath, string[] siteNames)
    {
        // Read existing entries from file
        List<string> existingEntries = new List<string>();
        if (File.Exists(filePath))
        {
            existingEntries.AddRange(File.ReadAllLines(filePath));
        }

        // Remove existing entries for duplicated site names
        foreach (string siteName in siteNames)
        {
            for (int i = existingEntries.Count - 1; i >= 0; i--)
            {
                if (existingEntries[i].StartsWith(siteName + ":"))
                {
                    existingEntries.RemoveAt(i);
                }
            }
        }

        // Write remaining entries back to file
        File.WriteAllLines(filePath, existingEntries);
    }
}
