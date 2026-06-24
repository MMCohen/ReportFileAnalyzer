using System;
using System.IO;
using System.Text;

namespace reportFile
{
    enum ReportType {
        intel,
        recon,
        analyze,
        collect
                    }

    enum Status {
        rejected,
        approved,
        pending
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

        static string[]? LoadFile()
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

            Console.WriteLine($"File loaded: {filereports.Length} lines found");
            return filereports;

        }

        static void ArreyPrint(string[] data)
        {
            for (int i = 0; i < data.Length; i++)
            {
                Console.WriteLine(data[i]);
            }
        }
        

        static void ProcessReports(string[] allLinesReport)
        {
            for (int i = 0; i < allLinesReport.Length; i++)
            {
                string Unit = "";
                ReportType Report;
                int Priority;
                double Score;
                Status status;

                string invalidRecords = "invalid records: ";
                bool isVAlid = true;

                string[] oneLine = allLinesReport[i].Split(",");

                if (oneLine.Length != 5)
                {
                    Console.WriteLine($"invalid record. line #:{i + 1}");
                    isVAlid = false;
                    continue;
                }

                oneLine = DeleteSpaces(oneLine);

                // check that unit is not empty
                if (oneLine[0].Length < 0)
                {
                    invalidRecords += "unit not enterd. ";
                    isVAlid = false;
                }
                else
                {
                    Unit = oneLine[0];
                }

                if (!Enum.TryParse(oneLine[1].ToLower(), out Report))
                {
                    invalidRecords += "invalid ReportType. ";
                    isVAlid = false;
                }

                if (!int.TryParse(oneLine[2], out Priority) || Priority < 1 || Priority > 5)
                {
                    invalidRecords += "invalid Priority. ";
                    isVAlid = false;
                }

                if (!double.TryParse(oneLine[3], out Score) || Score < 0.0 || Score > 100.0)
                {
                    invalidRecords += "invalid Score. ";
                    isVAlid = false;
                }

                if (!Enum.TryParse(oneLine[4].ToLower(), out status))
                {
                    isVAlid = false;
                    invalidRecords += "invalid status. ";
                }

                if (!isVAlid)
                {
                    Console.WriteLine(invalidRecords);
                    continue;
                }

                invalidRecords += 1;

                //addToArrays(Unit, Report, Priority, Score, status);
                Console.WriteLine($"Unit {Unit}, Report {Report}, Priority {Priority} , Score {Score}, status {status}");
                 



                // UNDONE:
            }
        }

        static string[] DeleteSpaces(string[] oneReport)
        {
            string[] newReport = new string[5];

            for (int i = 0; i < oneReport.Length; i++)
            {
                newReport[i] = oneReport[i].Trim();
            }
            return newReport;
        }

        static void Main()
        {
            DebugPrinting("hello from main");

            string[]? x = LoadFile();

            ReportType[] reportTypeArrey;
            Status[] StatusArrey;
            string[] UnitNameArrey;
            int[] PriorityArrey;
            double[] ScoreArrey;

            //ParsingReports();

            ProcessReports(x);

        }
    }
}