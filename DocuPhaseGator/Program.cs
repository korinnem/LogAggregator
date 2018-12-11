using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text.RegularExpressions;

namespace LogIngest
{
    class Program
    {
        static void Main(string[] args)
        {
            String logDir = @"C:\ProgramData\Docuphase\tracelogs";
            List<LogLine> allLogLines = new List<LogLine>();
            allLogLines = readLogs(logsExist(logDir));
            List<LogLine> sortedList = sortList(allLogLines);
            writeLogs(sortedList);

        }



        // --- Checks the default location for logs that are accessible ---
        static List<String> logsExist(String logDir)
        {
            List<String> logList = new List<String>();

            string[] getFiles = Directory.GetFiles(logDir);

            foreach (string name in getFiles)
            {
                // --- strip out the path to the file from the filename --- 
                // TODO: change this so that it works in the case of a custom logPath
                // instead of hard coding the removal of the path
                string logName = name.Substring(35);

                // --- add filename to the list ---
                logList.Add(logName);
            }

            if (logList.Count() > 0)
            {
                foreach (var logName in logList)
                {
                    String logTargetPath = @"C:\tracelogs\";

                    String logSourceDir = logDir + '\\' + logName;
                    String logTargetDir = logTargetPath + logName;

                    if (!System.IO.Directory.Exists(logTargetPath))
                    {
                        System.IO.Directory.CreateDirectory(logTargetPath);
                    }
                    System.IO.File.Copy(logSourceDir, logTargetDir, true);
                }

                return logList;
            }
            else
            {
                Console.WriteLine("There are no log files in the tracelogs folder");
                return null;
            }
        }

        // --- use the logList to read all logs into a single List<LogLine> --- 
        static List<LogLine> readLogs(List<String> logList)
        {
            String logHolding = "";
            String logLocation = "";
            List<LogLine> listOfLogs = new List<LogLine>();
            for (int i = 0; i < logList.Count; i++)
            {
                logLocation = (@"C:\tracelogs\" + logList[i]);
                foreach (string line in File.ReadLines(logLocation))
                {
                    String trimLine = line.Trim();
                    if (trimLine == null || trimLine == "  " || trimLine == "" || trimLine == " " || trimLine == "   ")
                    {
                        //do nothing
                    }
                    else if (trimLine != null)
                    {
                        String regexMatch = @"\d{4}";
                        if (trimLine.Length > 3)
                        {
                            if (Regex.IsMatch(trimLine.Substring(0, 4), regexMatch))
                            {
                                if (logHolding != "")
                                {
                                    LogLine pushLog = new LogLine(logHolding, logList[i]);
                                    listOfLogs.Add(pushLog);
                                    logHolding = "";

                                }
                                logHolding = trimLine;
                            }
                            else
                            {
                                logHolding += "\n\t" + trimLine;
                            }
                        }
                    }
                    else
                    {
                        LogLine pushLog = new LogLine(logHolding, logList[i]);
                        listOfLogs.Add(pushLog);
                        logHolding = "";

                    }
                }
            }
            return listOfLogs;
        }

        static List<LogLine> sortList(List<LogLine> allLogs)
        {
            List<LogLine> sorted = allLogs.OrderBy(logLine => logLine.getLogTime()).ToList();
            return sorted;
        }

        static void writeLogs(List<LogLine> allLogs)
        {
            using (TextWriter tw = new StreamWriter(@"C:\tracelogs\chronologs.txt"))
            {
                for (int i = 0; i < allLogs.Count; i++)
                {
                    tw.WriteLine(allLogs[i].getLog());
                }

                tw.Close();
            }

        }
    }
}
