using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simple_wordlist_generator
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 2)
            {
                Console.WriteLine("Error: Less than 2 arguments provided.");
                Console.WriteLine("Usage: snowswgen.exe [input] [output]");
                return;
            }

            string inputFileName = args[0];
            string outputFileName = args[1];

            UpperCaseEveryLetter1By1(inputFileName, outputFileName);

            Console.WriteLine("Done.");
        }

        static void UpperCaseEveryLetter1By1(string _input, string _output)
        {
            string[] lines = File.ReadAllLines(_input);

            for (int i = 0; i < lines.Length; i++)
            {
                int letterCount = 0;

                for (int y = 0; y < lines[i].Length; y++)
                {
                    if (char.IsLetter(lines[i][y]))
                        letterCount++;
                }

                //skip lines with more than 20 letters in order to avoid too long recursions
                if (Math.Pow(2, letterCount) > Math.Pow(2, 20))
                {
                    File.AppendAllLines(_output, new string[1]{lines[i]});
                    Console.WriteLine($"Skipped line {i}/{lines.Length - 1} due to being too long");
                    continue;
                }

                Console.WriteLine($"Processing line {i}/{lines.Length - 1} {(int)((float)i / (lines.Length - 1) * 100f)}%");

                List<string> resultLines = new List<string>();

                for (int x = 0; x < Math.Pow(2,lines[i].Length); x++)
                {
                    if (x >= Math.Pow(2, letterCount))
                        break;

                    string mask = Convert.ToString(x, 2).PadLeft(letterCount, '0');

                    string result = "";

                    int letterIndex = 0;

                    for (int y = 0; y < lines[i].Length; y++)
                    {
                        if (!char.IsLetter(lines[i][y]))
                        {
                            result += lines[i][y];
                            continue;
                        }

                        if (mask[letterIndex] == '1')
                            result += ReverseCase(lines[i][y]);
                        else
                            result += lines[i][y];

                        letterIndex++;
                    }

                    resultLines.Add(result);
                }

                File.AppendAllLines(_output, resultLines);
            }
        }

        static char ReverseCase(char _input)
        {
            if (char.IsLetter(_input))
            {
                if (char.IsLower(_input))
                    return char.ToUpper(_input);
                else
                    return char.ToLower(_input);
            }

            return _input;
        }
    }
}
