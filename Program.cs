using System;
using System.IO;
using System.Text;

namespace reportFile
{
    enum ReportType {
        Intel,
        Recon,
        Analyze,
        Collect
                    }

    enum Status {
        Rejected,
        Approved,
        Pending
                }


    class reportFileAnalayzer
    {
        static string FILEPATH = "reports.txt";


        /// <summary>
        /// printing func for debug purpose
        /// </summary>
        /// <param name="input"> the text you want tp print to the console </param>
        static void DebugPrinting(string input)
        {
            Console.WriteLine(input);
        }

        static string[]? LoadReports()
        {
            string[] filereports;
            try
            {
                filereports = File.ReadAllLines(FILEPATH, encoding: Encoding.UTF8);
                
                if (filereports.Length == 0)
                {
                    Console.WriteLine("Error: File is empty.");
                    return null;
                }
            }

            catch (EndOfStreamException) // catch (FileNotFoundException)
            {
                Console.WriteLine($"Error: File {FILEPATH} is not found."); // TODO: change the file path for only the actaualy file name and not the full path.
                return null;
            }


            return filereports;

        }

        static void ArreyPrint(string[] data)
        {
            for (int i = 0; i < data.Length; i++)
            {
                Console.WriteLine(data[i]);
            }
        }
        static void Main()
        {
            DebugPrinting("hello from main");

            string[]? x = LoadReports();
            

        }
    }
}