using System;
using System.ComponentModel.Design;
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
        const int ARRAY_SIZE = 100;
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

            catch (FileNotFoundException)
            {
                Console.WriteLine($"Error: File {FILEPATH} is not found."); // TODO: change the file path for only the actaualy file name and not the full path.
                return null;
            }

            Console.WriteLine($"File loaded: {filereports.Length} lines found");
            return filereports;

        }

        static void ArrayPrint(string[] data, int validRecords)
        {
            for (int i = 0; i < validRecords; i++)
            {
                Console.WriteLine(data[i]);
            }
        }
        static void ArrayPrint(int[] data, int validRecords)
        {
            for (int i = 0; i < validRecords; i++)
            {
                Console.WriteLine(data[i]);
            }
        }
        static void ArrayPrint(double[] data, int validRecords)
        {
            for (int i = 0; i < validRecords; i++)
            {
                Console.WriteLine(data[i]);
            }
        }
        static void ArrayPrint(ReportType[] data, int validRecords)
        {
            for (int i = 0; i < validRecords; i++)
            {
                Console.WriteLine(data[i]);
            }
        }
        static void ArrayPrint(Status[] data, int validRecords)
        {
            for (int i = 0; i < validRecords; i++)
            {
                Console.WriteLine(data[i]);
            }
        }


        static void ProcessReports(string[] allLinesReport, ref int validRecords, string[] unitArr, ReportType[] reportArr, int[] priorityArr, double[] scoreArr, Status[] statusArr )
        {
            //int sumValidRecords = 0; // also used to know the index of last report exist

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

                if (!Enum.TryParse(oneLine[1].ToLower(), out Report))  // TODO: use gnoreCase: true
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

                if (!Enum.TryParse(oneLine[4].ToLower(), out status)) // TODO: use gnoreCase: true
                {
                    isVAlid = false;
                    invalidRecords += "invalid status. ";
                }

                if (!isVAlid)
                {
                    Console.WriteLine(invalidRecords);
                    continue;
                }

                

                addToArray(Unit, unitArr, validRecords);
                addToArray(Report, reportArr, validRecords);
                addToArray(Priority, priorityArr, validRecords);
                addToArray(Score, scoreArr, validRecords);
                addToArray(status, statusArr, validRecords);

                validRecords += 1;

                Console.WriteLine($"Unit {Unit}, Report {Report}, Priority {Priority} , Score {Score}, status {status}");
                Console.WriteLine($"Valid record processed.");

                // UNDONE:
            }
        }


        static void addToArray(string unit, string[] unitArr, int index)
        {
            unitArr[index] = unit;
        }
        static void addToArray(ReportType report, ReportType[] reportArr, int index)
        {
            reportArr[index] = report;
        }
        static void addToArray(int priority, int[] priorityArr, int index)
        {
            priorityArr[index] = priority;
        }
        static void addToArray(double score, double[] scoreArr, int index)
        {
            scoreArr[index] = score;
        }
        static void addToArray(Status status, Status[] statusArr, int index)
        {
            statusArr[index] = status;
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


        static double CalculateAverage(double[] scoreArr, int validRecords)
        {
            double acc = 0;
            for (int i = 0; i < validRecords; i++)
            {
                acc += scoreArr[i];
            }
            return (acc / validRecords);
        }

        static double FindMaxScore(double[] scoreArr, int validRecords)
        {
            double maxScore = scoreArr[0];

            for (int i = 0; i < validRecords; i++)
            {
                if (scoreArr[i] > maxScore)
                {
                    maxScore = scoreArr[i];
                }
            }
            return maxScore;
        }

        static double FindMinScore(double[] scoreArr, int validRecords)
        {
            double minScore = scoreArr[0];

            for (int i = 0; i < validRecords; i++)
            {
                if (scoreArr[i] < minScore)
                {
                    minScore = scoreArr[i];
                }
            }
            return minScore;
        }


        static int CountByStatus(Status[] statusArr, int validRecords, string status)
        {
            int cnt = 0;
            
            for (int i = 0; i < validRecords; i++)
            {
                if (Enum.TryParse(status, ignoreCase: true, out Status chek) && chek == statusArr[i])
                {
                    cnt++;
                }
            }
            return cnt;
        }
        

        
        static void Main()
        {
            DebugPrinting("hello from main");

            string[]? x = LoadFile();

            string[] UnitNameArrey = new string[ARRAY_SIZE];
            ReportType[] reportTypeArrey = new ReportType[ARRAY_SIZE];
            int[] PriorityArrey = new int[ARRAY_SIZE];
            double[] ScoreArrey = new double[ARRAY_SIZE];
            Status[] StatusArrey = new Status[ARRAY_SIZE];

            int validRecords = 0;
            int invalidRecords; // TODO: use it


            ProcessReports(x, ref validRecords, UnitNameArrey, reportTypeArrey, PriorityArrey, ScoreArrey, StatusArrey);

            Console.WriteLine("============================");
            ArrayPrint(UnitNameArrey, validRecords);

            double average = CalculateAverage(ScoreArrey, validRecords);
            Console.WriteLine($"{average:F2}");

            double minscore = FindMinScore(ScoreArrey, validRecords);
            Console.WriteLine(minscore);

            int cnt_by_stat = CountByStatus(StatusArrey, validRecords, "Pending");
            Console.WriteLine("cnt_by_stat: " + cnt_by_stat);


        }
    }
}