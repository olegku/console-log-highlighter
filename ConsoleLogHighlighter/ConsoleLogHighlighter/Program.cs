using System;
using System.IO;

namespace ConsoleLogHighlighter
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var reader = new StreamReader("Sample.log"))
            {
                Console.SetIn(reader);

                using (new ConsoleLogHighlighter())
                {
                    while (Console.ReadLine() is string line)
                    {
                        Console.WriteLine(line);
                    }
                }

                Console.OpenStandardOutput();
            }
        }
    }
}
