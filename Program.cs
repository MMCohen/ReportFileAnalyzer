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


        static void ProcessReports(string[] allLinesReport, ref int validRecords, ref int invalidRecords, string[] unitArr, ReportType[] reportArr, int[] priorityArr, double[] scoreArr, Status[] statusArr )
        {
            //int sumValidRecords = 0; // also used to know the index of last report exist

            for (int i = 0; i < allLinesReport.Length; i++)
            {
                string Unit = "";
                ReportType Report;
                int Priority;
                double Score;
                Status status;

                string invalidRecordsMessage = "invalid records: ";
                bool isVAlid = true;

                string[] oneLine = allLinesReport[i].Split(",");

                if (oneLine.Length != 5)
                {
                    Console.WriteLine($"invalid record. line #:{i + 1}");
                    invalidRecords += 1;
                    continue;
                }

                oneLine = DeleteSpaces(oneLine);

                // check that unit is not empty
                if (oneLine[0].Length < 0)
                {
                    invalidRecordsMessage += "unit not enterd. ";
                    isVAlid = false;
                }
                else
                {
                    Unit = oneLine[0];
                }

                if (!Enum.TryParse(oneLine[1].ToLower(), out Report))  // TODO: use gnoreCase: true
                {
                    invalidRecordsMessage += "invalid ReportType. ";
                    isVAlid = false;
                }

                if (!int.TryParse(oneLine[2], out Priority) || Priority < 1 || Priority > 5)
                {
                    invalidRecordsMessage += "invalid Priority. ";
                    isVAlid = false;
                }

                if (!double.TryParse(oneLine[3], out Score) || Score < 0.0 || Score > 100.0)
                {
                    invalidRecordsMessage += "invalid Score. ";
                    isVAlid = false;
                }

                if (!Enum.TryParse(oneLine[4].ToLower(), out status)) // TODO: use gnoreCase: true
                {
                    isVAlid = false;
                    invalidRecordsMessage += "invalid status. ";
                }

                if (!isVAlid)
                {
                    Console.WriteLine(invalidRecordsMessage);
                    invalidRecords += 1;
                    continue;
                }
                

                addToArray(Unit, unitArr, validRecords);
                addToArray(Report, reportArr, validRecords);
                addToArray(Priority, priorityArr, validRecords);
                addToArray(Score, scoreArr, validRecords);
                addToArray(status, statusArr, validRecords);

                validRecords += 1;

                Console.WriteLine($"Unit: {Unit}, Report: {Report}, Priority: {Priority} , Score: {Score}, status: {status}");
                Console.WriteLine($"Valid record processed.");

            }
            Console.WriteLine($"valid records: {validRecords}.\ninvalid records: {invalidRecords}");
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


        static void DisplayHighestPriorityApproved(string[] unitArr, ReportType[] reportArr, int[] priorityArr, double[] scoreArr, Status[] statusArr, int validRecords)
        {
            int highestPriority = FindHighestPriority(priorityArr, validRecords);

            Console.WriteLine("=== Display Highest Priority Approved ===\n");
            
            for (int i = 0; i < validRecords; i++)
            {
                if (priorityArr[i] == highestPriority && statusArr[i] == Status.approved)
                {
                    string reportRecord = getRecordByIndex(i, unitArr, reportArr, priorityArr, scoreArr, statusArr);
                    Console.WriteLine(reportRecord + "\n");
                }
            }
            Console.WriteLine("===============================\n");

        }

        static void DisplayAverageByPriority(int[] priorityArr, double[] scoreArr, int validRecords)
        {
            // 2 lists will be a much better choice, but we are not allow to use lists in this stage.
            double priority1ScoreAcc = 0;
            double priority2ScoreAcc = 0;
            double priority3ScoreAcc = 0;
            double priority4ScoreAcc = 0;
            double priority5ScoreAcc = 0;

            int priority1Cnt = 0;
            int priority2Cnt = 0;
            int priority3Cnt = 0;
            int priority4Cnt = 0;
            int priority5Cnt = 0;

            for (int i = 0; i < validRecords; i++)
            {
                switch (priorityArr[i])
                {
                    case 1:
                        priority1ScoreAcc += scoreArr[i];
                        priority1Cnt += 1;
                        break;
                    case 2:
                        priority2ScoreAcc += scoreArr[i];
                        priority2Cnt += 1;
                        break;
                    case 3:
                        priority3ScoreAcc += scoreArr[i];
                        priority3Cnt += 1;
                        break;
                    case 4:
                        priority4ScoreAcc += scoreArr[i];
                        priority4Cnt += 1;
                        break;
                    case 5:
                        priority5ScoreAcc += scoreArr[i];
                        priority5Cnt += 1;
                        break;


                }
            }

            Console.WriteLine("\n===================================");
            Console.WriteLine("=== Display Average By Priority ===");
            Console.WriteLine("===================================\n");
            if (priority1Cnt > 0)
            {
                Console.WriteLine($"priority 1 average score is: {priority1ScoreAcc / priority1Cnt}");
            }
            else
            {
                Console.WriteLine($"no reports for priority 1.");
            }

            if (priority2Cnt > 0)
            {
                Console.WriteLine($"priority 2 average score is: {priority2ScoreAcc / priority2Cnt}");
            }
            else
            {
                Console.WriteLine($"no reports for priority 2.");
            }
            
            if (priority3Cnt > 0)
            {
                Console.WriteLine($"priority 3 average score is: {priority3ScoreAcc / priority3Cnt}");
            }
            else
            {
                Console.WriteLine($"no reports for priority 3.");
            }
            
            if (priority4Cnt > 0)
            {
                Console.WriteLine($"priority 4 average score is: {priority4ScoreAcc / priority4Cnt}");
            }
            else
            {
                Console.WriteLine($"no reports for priority 4.");
            }
            
            if (priority5Cnt > 0)
            {
                Console.WriteLine($"priority 5 average score is: {priority5ScoreAcc / priority5Cnt}");
            }
            else
            {
                Console.WriteLine($"no reports for priority 5.");
            }
            Console.WriteLine("===================================\n");


        }


        static void Main()
        {
            string[]? Lines = LoadFile();
            
            string[] UnitNameArray = new string[ARRAY_SIZE];
            ReportType[] reportTypeArray = new ReportType[ARRAY_SIZE];
            int[] PriorityArray = new int[ARRAY_SIZE];
            double[] ScoreArray = new double[ARRAY_SIZE];
            Status[] StatusArray = new Status[ARRAY_SIZE];

            int validRecords = 0;
            int invalidRecords = 0;

            if (Lines == null)
            {
                Console.WriteLine("No records found.");
                return;
            }

            ProcessReports(Lines, ref validRecords, ref invalidRecords, UnitNameArray, reportTypeArray, PriorityArray, ScoreArray, StatusArray);

            DisplayBasicStatistics(ScoreArray, validRecords);

            DisplayStatusCounts(StatusArray, validRecords);

            DisplayTypeCounts(reportTypeArray, validRecords);

            DisplayHighestPriorityApproved(UnitNameArray, reportTypeArray, PriorityArray, ScoreArray, StatusArray, validRecords);

            DisplayAverageByPriority(PriorityArray, ScoreArray, validRecords);

            
        }
    }
}