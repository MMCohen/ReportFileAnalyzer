using System.IO;

namespace reportFile
{
    class reportFileAnalayzer
    {
        static void DebugPrinting(string input) // for debug purpose
        {
            Console.WriteLine(input);
        }

        static List<string> LoadReports()
        {
            // need to check if file exist

            string[] filereports = File.ReadAllLines("reports.txt");


        }
        static void Main()
        {


        }
    }
}