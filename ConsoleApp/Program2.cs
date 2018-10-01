using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ConsoleApp
{
    public class Program2
    {
        public static void Main2(string[] args)
        {

            var lines = new List<string>();

            string line;
            while (!string.IsNullOrWhiteSpace(line = Console.ReadLine()))
            {
                lines.Add(line);
            }

            var tempFileName = Path.GetTempFileName();

            var filePath1 = Path.Combine(Directory.GetCurrentDirectory(), "output", "file1.txt");
            Directory.CreateDirectory(Path.GetDirectoryName(filePath1));

            File.WriteAllLines(filePath1, lines.ToArray());

            var filePath2 = Path.Combine(Directory.GetCurrentDirectory(), "output", "file2.txt");
            Directory.CreateDirectory(Path.GetDirectoryName(filePath2));

            using (var file = File.CreateText(filePath2))
            {
                foreach (var outputLine in lines)
                {
                    file.WriteLine(line);
                }
            }

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}
