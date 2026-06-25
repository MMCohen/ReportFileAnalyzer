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


        static int CountByType(ReportType[] reportArr, int validRecords, string reportString)
        {
            int cnt = 0;

            for (int i = 0; i < validRecords; i++)
            {
                if (Enum.TryParse(reportString, ignoreCase: true, out ReportType chek) && chek == reportArr[i])
                {
                    cnt++;
                }
            }
            return cnt;
        }


        static void DisplayBasicStatistics(double[] scoreArr, int validRecords)
        {
            Console.WriteLine("================================");
            Console.WriteLine("=== Display Basic Statistics ===");
            Console.WriteLine("================================\n");
            Console.WriteLine($"Valid records: {validRecords}");

            double avg = CalculateAverage(scoreArr, validRecords);
            double max = FindMaxScore(scoreArr, validRecords);
            double min = FindMinScore(scoreArr, validRecords);

            Console.WriteLine($"Avarage score: {avg:F2}\nMax score: {max}\nMin score: {min}");
            Console.WriteLine("================================\n");
        }


        static void DisplayStatusCounts(Status[] statusArr, int validRecords)
        {

            int pendingCnt = CountByStatus(statusArr, validRecords, "pending");
            int approvedCnt = CountByStatus(statusArr, validRecords, "approved");
            int rejectedCnt = CountByStatus(statusArr, validRecords, "rejected");

            Console.WriteLine("=============================");
            Console.WriteLine("=== Display Status Counts ===");
            Console.WriteLine("=============================\n");

            Console.WriteLine($"Pending count: {pendingCnt}.\napproved count: {approvedCnt}.\nRejected count: {rejectedCnt}.\n");

            Console.WriteLine("=============================\n");
        }


        static void DisplayTypeCounts(ReportType[] reportArr, int validRecords)
        {
            int CollectCnt = CountByType(reportArr, validRecords, "Collect");
            int AnalyzeCnt = CountByType(reportArr, validRecords, "Analyze");
            int ReconCnt = CountByType(reportArr, validRecords, "Recon");
            int IntelCnt = CountByType(reportArr, validRecords, "Intel");

            Console.WriteLine("===========================");
            Console.WriteLine("=== Display Type Counts ===");
            Console.WriteLine("===========================\n");

            Console.WriteLine($"Collect count: {CollectCnt}.\nAnalyze count: {AnalyzeCnt}.\nRecon count: {ReconCnt}.\nIntel count:{IntelCnt}\n");

            Console.WriteLine("=============================\n");
        }


        static int FindHighestPriority(int[] priorityArr, int validRecords)
        {
            int highestPriority = priorityArr[0];

            for (int i = 0; i < validRecords; i++)
            {
                if (priorityArr[i] > highestPriority)
                {
                    highestPriority = priorityArr[i];
                }
            }
            return highestPriority;
        }



        static string getRecordByIndex(int index, string[] unitArr, ReportType[] reportArr, int[] priorityArr, double[] scoreArr, Status[] statusArr)
        {
            string ReportRecord = $"""
                unit: {unitArr[index]}. 
                report: {reportArr[index]}.
                priority: {priorityArr[index]}.
                score: {scoreArr[index]}.
                status: {statusArr[index]}.
                """;

            return ReportRecord;
        }


        static void Main()
        {
            DebugPrinting("hello from main");

            string[]? x = LoadFile();

            string[] UnitNameArray = new string[ARRAY_SIZE];
            ReportType[] reportTypeArray = new ReportType[ARRAY_SIZE];
            int[] PriorityArray = new int[ARRAY_SIZE];
            double[] ScoreArray = new double[ARRAY_SIZE];
            Status[] StatusArray = new Status[ARRAY_SIZE];

            int validRecords = 0;
            int invalidRecords; // TODO: use it


            ProcessReports(x, ref validRecords, UnitNameArray, reportTypeArray, PriorityArray, ScoreArray, StatusArray);

            Console.WriteLine("============================");
            ArrayPrint(UnitNameArray, validRecords);

            double average = CalculateAverage(ScoreArray, validRecords);
            Console.WriteLine($"{average:F2}");

            double minscore = FindMinScore(ScoreArray, validRecords);
            Console.WriteLine(minscore);

            int cnt_by_stat = CountByStatus(StatusArray, validRecords, "Pending");
            Console.WriteLine("cnt_by_stat: " + cnt_by_stat);

            int cnt_by_type = CountByType(reportTypeArray, validRecords, "anaLyze");
            Console.WriteLine("cnt_by_type: " + cnt_by_type);

            Console.WriteLine("============================");
            DisplayBasicStatistics(ScoreArray, validRecords);

            Console.WriteLine("============================");
            DisplayStatusCounts(StatusArray, validRecords);

            Console.WriteLine("============================");
            DisplayTypeCounts(reportTypeArray, validRecords);
        }
    }
}